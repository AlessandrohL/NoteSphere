﻿using System;
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
        ClaimsPrincipal ValidateToken(string accessToken);
        Task<ClaimsIdentity> ValidateTokenAsync(string accessToken);

    }
}
