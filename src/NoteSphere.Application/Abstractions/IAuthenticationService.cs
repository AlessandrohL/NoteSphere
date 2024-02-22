﻿using Application.Common;
using Application.DTOs.Token;
using Application.DTOs.User;
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
        Task<TokenResponse> RegisterUserAsync(
            UserRegistrationDto userRegistration,
            CancellationToken cancellationToken);
        Task<TokenResponse> LoginUserAsync(
            UserLoginDto userLogin,
            CancellationToken cancellationToken);
        Task<TokenResponse> RefreshTokenAsync(
            TokenRefreshRequest tokenRefreshRequest,
            CancellationToken cancellationToken);

    }
}
