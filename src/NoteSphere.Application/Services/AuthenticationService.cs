using Application.Abstractions;
using Application.DTOs.Token;
using Application.DTOs.User;
using Application.Email;
using Application.Exceptions;
using Application.Helpers;
using Application.Identity;
using AutoMapper;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService<UserAuth>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<UserAuth> _userManager;
        private readonly IUserManagerExtensions<UserAuth> _userManagerExtensions;
        private readonly IJwtService _jwtService;
        private readonly IUrlUtility _urlUtility;
        private readonly IEmailService _emailService;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;

        public AuthenticationService(
            IApplicationUserRepository applicationUserRepository,
            IUnitOfWork unitOfWork,
            UserManager<UserAuth> userManager,
            IUserManagerExtensions<UserAuth> userManagerExtensions,
            IJwtService jwtService,
            IUrlUtility urlUtility,
            IEmailService emailService,
            ITenantService tenantService,
            IMapper mapper)
        {
            _applicationUserRepository = applicationUserRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userManagerExtensions = userManagerExtensions;
            _jwtService = jwtService;
            _urlUtility = urlUtility;
            _emailService = emailService;
            _mapper = mapper;
            _tenantService = tenantService;
        }

        public async Task<TokensResponse> LoginUserAsync(
            UserLoginDto userLogin, 
            CancellationToken cancellationToken)
        {
            var identityUser = await _userManager.FindByEmailAsync(userLogin.Email!);

            if (identityUser is null)
            {
                throw new IdentityUserNotFoundException();
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(
                user: identityUser, 
                password: userLogin.Password!);

            if (!isValidPassword)
            {
                throw new UserInvalidCredentialsException();
            }
            
            if (!identityUser.EmailConfirmed)
            {
                throw new UnconfirmedUserEmailException();
            }

            var tenantClaim = await _tenantService.GetTenantFromUser(identityUser);
            var tenantId = Guid.Parse(tenantClaim!.Value);

            var claims = GenerateClaimsForUser(identityUser, tenantId, identityUser.EmailConfirmed);

            var accessToken = _jwtService.CreateToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiration = _jwtService.GenerateExpirationDateForRefreshToken();

            identityUser.RefreshToken = refreshToken;
            identityUser.RefreshTokenExpiryTime = refreshTokenExpiration;

            await _userManager.UpdateAsync(identityUser);

            return new TokensResponse(accessToken, refreshToken);
        }

        public async Task RegisterUserAsync(
            UserRegistrationDto userRegistration,
            CancellationToken cancellationToken)
        {
            var emailInUse = await _userManagerExtensions.IsEmailAlreadyInUseAsync(
                email: userRegistration.Email!, 
                cancellationToken);

            if (emailInUse)
            {
                throw new UserAlreadyExistsException(new string[] 
                {
                    "The email address is already in use. Please choose a different email address."
                });
            }

            var usernameInUse = await _userManagerExtensions.IsUsernameAlreadyInUseAsync(
                username: userRegistration.UserName!,
                cancellationToken);

            if (usernameInUse)
            {
                throw new UserAlreadyExistsException(new string[]
                {
                    "The username is already taken. Please choose a different username."
                });
            }

            var identityUser = _mapper.Map<UserAuth>(userRegistration);
            var tenant = _tenantService.CreateTenant();

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var identityResult = await _userManager.CreateAsync(
                    identityUser,
                    userRegistration.Password!);

                if (!identityResult.Succeeded)
                {
                    var errors = identityResult.Errors
                        .Select(e => e.Description);

                    throw new Exception(string.Join(',', errors));
                }

                await _userManager.AddClaimAsync(identityUser, tenant.ToClaim());

                var appUser = _mapper.Map<ApplicationUser>(userRegistration);
                appUser.AssignTenant(tenant);

                _applicationUserRepository.Create(appUser);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var confirmationUrl = _emailService.GenerateEmailConfirmationUrl(identityUser.Id, confirmationToken);
            var body = _emailService.CreateEmailConfirmationTemplate(userRegistration.FirstNames!, confirmationUrl);
            var message = new Message(
                to: new string[] { identityUser.Email! },
                subject: EmailConstants.SubjectConfirmationEmail, 
                content: body);

            await _emailService.SendEmailConfirmationAsync(message);
        }

        public async Task ConfirmUserEmailAsync(EmailConfirmationRequest confirmationRequest)
        {
            var identityUser = await _userManager.FindByIdAsync(confirmationRequest.Id!);

            if (identityUser is null)
            {
                throw new IdentityUserNotFoundException();
            }

            if (identityUser.EmailConfirmed)
            {
                throw new UserAlreadyConfirmedException();
            }

            var decodedCode = _urlUtility.DecodeBase64Url(confirmationRequest.Code!);
            var result = await _userManager.ConfirmEmailAsync(identityUser, decodedCode);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(e => e.Description)
                    .ToArray();

                throw new UserValidationException(errors);
            }

        }

        public async Task LogoutUserAsync(string? accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidAccessTokenException();
            }

            var claims = _jwtService.GetClaimsFromToken(accessToken)?.ToList();
            var subject = claims?.FirstOrDefault(c => c.Type == GeneralClaimTypes.Subject);

            var user = await _userManager.FindByIdAsync(subject!.Value);
            
            if (user is null)
            {
                throw new IdentityUserNotFoundException();
            }

            await RemoveRefreshTokenAsync(user);
        }

        public async Task<RefreshTokenResponse> RefreshAccessTokenAsync(
            string? accessToken,
            RefreshTokenRequest tokenRefreshRequest,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidAccessTokenException("accessToken is null or empty.");
            }

            var claimsIdentity = await _jwtService.ValidateTokenAsync(accessToken);
            var subject = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(subject.Value);

            if (user is null)
            {
                throw new IdentityUserNotFoundException();
            }

            if (user.RefreshToken is null)
            {
                throw new RefreshTokenNotSetException();
            }

            if (user.RefreshToken != tokenRefreshRequest.RefreshToken)
            {
                throw new InvalidRefreshTokenException();
            }

            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new RefreshTokenExpiredException();
            }

            var userTenantClaim = await _tenantService.GetTenantFromUser(user);
            var tenantId = Guid.Parse(userTenantClaim!.Value);

            var claims = GenerateClaimsForUser(user, tenantId, user.EmailConfirmed);

            var renewedAccessToken = _jwtService.CreateToken(claims);

            return new RefreshTokenResponse(renewedAccessToken);
        }

        public Task RemoveRefreshTokenAsync(UserAuth user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            return _userManager.UpdateAsync(user);
        }

        private static List<Claim> GenerateClaimsForUser(
            IUserWithIdentityFeatures user, 
            Guid tenantId,
            bool confirmedAccount = false)
        {
            return new List<Claim>()
            {
               new (GeneralClaimTypes.Subject, user.Id),
               new (GeneralClaimTypes.Name, user.UserName!),
               new (GeneralClaimTypes.Email, user.Email!),
               new (GeneralClaimTypes.Tenant, tenantId.ToString()),
               new (GeneralClaimTypes.Confirmed, confirmedAccount.ToString())
            };
        }
    }
}
