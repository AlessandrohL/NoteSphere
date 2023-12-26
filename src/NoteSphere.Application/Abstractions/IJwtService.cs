using Application.Models.Common;
using Application.Models.DTOs.Token;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IJwtService
    {
        string CreateToken(IEnumerable<Claim> claims);
        (string, DateTime) GenerateRefreshTokenAndExpiryTime();
        ClaimsPrincipal ValidateToken(string token);
        Task<Result<ClaimsIdentity, string>> ValidateTokenAsync(string accessToken);

    }
}
