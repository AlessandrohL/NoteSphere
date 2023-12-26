using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public class JwtConfigHelper
    {
        private readonly IConfiguration _config;

        public JwtConfigHelper(IConfiguration config)
            => _config = config;


        public string? GetIssuer() => _config["JwtSettings:ValidIssuer"];

        public string? GetAudience() => _config["JwtSettings:ValidAudience"];

        public SigningCredentials GetSigningCredentials() =>
            new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

        public string? GetSecurityKey() => _config["JwtSettings:Secret"]; 

        public byte[] GetSymmetricSecurityKeyAsBytes() => Encoding.UTF8.GetBytes(GetSecurityKey());

        public SymmetricSecurityKey GetSymmetricSecurityKey()
            => new (GetSymmetricSecurityKeyAsBytes());

        public double GetTokenValidityMinutes() 
            => Convert.ToDouble(_config["JwtSettings:TokenLifetimeMinutes"]);

        public double GetRefreshTokenValidityDays()
            => Convert.ToDouble(_config["JwtSettings:RefreshTokenLifetimeDays"]);
    }
}
