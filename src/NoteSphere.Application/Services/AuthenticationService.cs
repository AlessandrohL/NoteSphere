using Application.Abstractions;
using Application.Common;
using Application.DTOs.Token;
using Application.DTOs.User;
using Application.Errors;
using Application.Exceptions;
using Application.Helpers;
using Application.Identity;
using AutoMapper;
using Domain.Abstractions;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<UserAuth> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;

        public AuthenticationService(
            UserManager<UserAuth> userManager,
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IMapper mapper,
            ITenantService tenantService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
            _tenantService = tenantService;
        }

        public async Task<TokenResponse> LoginUserAsync(UserLoginDto userLogin)
        {
            var identityUser = await _userManager
                .FindByEmailAsync(userLogin.Email!);

            if (identityUser is null)
            {
                throw new IdentityUserNotFoundException();
            }

            var isValidPassword = await _userManager
                .CheckPasswordAsync(identityUser, userLogin.Password!);

            if (!isValidPassword)
            {
                throw new IdentityUserInvalidCredentialsException();
            }

            var tenantClaim = await _tenantService.GetTenantFromUser(identityUser);
            var tenantId = Guid.Parse(tenantClaim!.Value);

            var claims = _tenantService.GenerateClaimsForUser(identityUser, tenantId);

            var accessToken = _jwtService.CreateToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiration = _jwtService.GenerateExpirationDateForRefreshToken();

            identityUser.RefreshToken = refreshToken;
            identityUser.RefreshTokenExpiryTime = refreshTokenExpiration;

            await _userManager.UpdateAsync(identityUser);

            return new TokenResponse(accessToken, refreshToken);
        }

        public async Task<TokenResponse> RegisterUserAsync(
            UserRegistrationDto userRegistration)
        {
            var identityUser = _mapper.Map<UserAuth>(userRegistration);

            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiration = _jwtService.GenerateExpirationDateForRefreshToken();

            identityUser.RefreshToken = refreshToken;
            identityUser.RefreshTokenExpiryTime = refreshTokenExpiration;

            var identityResult = await _userManager.CreateAsync(
                identityUser,
                userRegistration.Password!);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors
                    .Select(e => e.Description)
                    .ToList();

                throw new IdentityUserValidationException(errors);
            }

            var tenant = _tenantService.CreateTenant();

            await _userManager.AddClaimAsync(identityUser, tenant.ToClaim());

            var appUser = _mapper.Map<ApplicationUser>(userRegistration);
            appUser.AssignTenant(tenant);

            _unitOfWork.ApplicationUser.Create(appUser);

            await _unitOfWork.SaveChangesAsync();

            var claims = _tenantService.GenerateClaimsForUser(identityUser, tenant);

            var accessToken = _jwtService.CreateToken(claims);

            return new TokenResponse(accessToken, refreshToken);
        }

        public async Task<TokenResponse> RefreshTokenAsync(TokenRefreshRequest tokenRefreshRequest)
        {
            var claimsIdentity = await _jwtService
                .ValidateTokenAsync(tokenRefreshRequest.AccessToken!);

            var subject = claimsIdentity
                .FindFirst(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(subject.Value);

            if (user is null)
            {
                throw new IdentityUserNotFoundException();
            }

            if (user.RefreshToken != tokenRefreshRequest.RefreshToken)
            {
                throw new InvalidRefreshTokenException();
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new RefreshTokenExpiredException();
            }

            var userTenantClaim = await _tenantService.GetTenantFromUser(user);
            var tenantId = Guid.Parse(userTenantClaim!.Value);

            var claims = _tenantService.GenerateClaimsForUser(user, tenantId);

            var accessToken = _jwtService.CreateToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return new TokenResponse(accessToken, refreshToken);
        }
    }
}
