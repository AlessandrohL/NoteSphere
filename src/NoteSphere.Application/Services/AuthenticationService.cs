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
        private readonly IMapper _mapper;

        public AuthenticationService(
            UserManager<UserAuth> userManager,
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
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

            var claims = GenerateClaimsForUser(
                identityUser.Id,
                identityUser.UserName!,
                identityUser.Email!);

            var accessToken = _jwtService.CreateToken(claims);
            var (refreshToken, expiryTime) = _jwtService.GenerateRefreshTokenAndExpiryTime();

            identityUser.RefreshToken = refreshToken;
            identityUser.RefreshTokenExpiryTime = expiryTime;

            await _userManager.UpdateAsync(identityUser);

            return new TokenResponse(accessToken, refreshToken);
        }

        public async Task<TokenResponse> RegisterUserAsync(
            UserRegistrationDto userRegistration)
        {
            var identityUser = _mapper.Map<UserAuth>(userRegistration);

            var (refreshToken, expiryTime) = _jwtService.GenerateRefreshTokenAndExpiryTime();

            identityUser.RefreshToken = refreshToken;
            identityUser.RefreshTokenExpiryTime = expiryTime;

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

            var appUser = _mapper.Map<ApplicationUser>(userRegistration);
            appUser.AssignIdentity(identityUser.Id);

            _unitOfWork.ApplicationUser.Create(appUser);

            await _unitOfWork.SaveChangesAsync();

            var claims = GenerateClaimsForUser(
                identityUser.Id,
                identityUser.UserName!,
                identityUser.Email!);

            var accessToken = _jwtService.CreateToken(claims);

            return new TokenResponse(accessToken, refreshToken);
        }

        private List<Claim> GenerateClaimsForUser(string id, string username, string email)
        {
            return new List<Claim>()
            {
               new (JwtClaimTypes.Subject, id),
               new (JwtClaimTypes.Name, username),
               new (JwtClaimTypes.Email, email)
            };
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

            var claims = GenerateClaimsForUser(
                user.Id,
                user.UserName!,
                user.Email!);

            var accessToken = _jwtService.CreateToken(claims);
            var (refreshToken, _) = _jwtService.GenerateRefreshTokenAndExpiryTime();

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return new TokenResponse(accessToken, refreshToken);
        }


    }
}
