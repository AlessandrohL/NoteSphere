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
        string GenerateRefreshToken();
        DateTime GenerateExpirationDateForRefreshToken();
        ClaimsPrincipal ValidateToken(string accessToken);
        Task<ClaimsIdentity> ValidateTokenAsync(string accessToken);
        IEnumerable<Claim>? GetClaimsFromToken(string accessToken);

    }
}
