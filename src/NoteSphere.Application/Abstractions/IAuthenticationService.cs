﻿using Application.Models.Common;
using Application.Models.DTOs.Token;
using Application.Models.DTOs.User;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IAuthenticationService<TUser> where TUser : IUserWithIdentityFeatures
    {
        Task<Result<TokenResponse, List<string>>> RegisterUserAsync(UserRegistrationDto userRegistration);
        Task<Result<TokenResponse, string>> LoginUserAsync(UserLoginDto userLogin);
        Task<Result<TokenResponse, string>> RefreshTokenAsync(TokenRefreshRequest tokenRefreshRequest);
        
    }
}
