using Application.Abstractions;
using Application.Exceptions;
using Infrastructure.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfigHelper _jwtConfig;

        public JwtService(JwtConfigHelper jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public string CreateToken(IEnumerable<Claim> claims)
        {
            var signInCredentials = _jwtConfig.GetSigningCredentials();
            var tokenOptions = GenerateTokenOptions(signInCredentials, claims);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return accessToken;
        }

        public (string, DateTime) GenerateRefreshTokenAndExpiryTime()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return (
                    Convert.ToBase64String(randomNumber),
                    DateTime.UtcNow.AddDays(_jwtConfig.GetRefreshTokenValidityDays()));
            }
        }

        /*
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenValidationParameters = GetValidationParameters();
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = tokenHandler
                .ValidateToken(token, tokenValidationParameters, out var validatedToken);

            return claims;
        }
        */

        public async Task<ClaimsIdentity> ValidateTokenAsync(string accessToken)
        {
            var validationParameters = GetValidationParameters();
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationResult = await tokenHandler
                .ValidateTokenAsync(accessToken, validationParameters);

            if (!validationResult.IsValid)
            {
                throw new InvalidAccessTokenException();
            }

            /*
            if (!validationResult.IsValid)
                return validationResult.Exception.Message;

            // Valido hasta (validTo).
            var validTo = validationResult.SecurityToken.ValidTo;
            */

            return validationResult.ClaimsIdentity;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _jwtConfig.GetSymmetricSecurityKey(),
                ValidateLifetime = true,
                ValidIssuer = _jwtConfig.GetIssuer(),
                ValidAudience = _jwtConfig.GetAudience()
            };
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
            IEnumerable<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
                (
                    issuer: _jwtConfig.GetIssuer(),
                    audience: _jwtConfig.GetAudience(),
                    claims,
                    expires: DateTime.UtcNow.AddMinutes
                        (_jwtConfig.GetTokenValidityMinutes()),
                    signingCredentials: signingCredentials
                );

            return tokenOptions;
        }
    }
}
