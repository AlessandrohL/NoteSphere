using Application.Common;
using Application.DTOs.Token;
using Application.DTOs.User;
using Application.Email;
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
        Task RegisterUserAsync(
            UserRegistrationDto userRegistration,
            CancellationToken cancellationToken);
        Task<TokensResponse> LoginUserAsync(
            UserLoginDto userLogin,
            CancellationToken cancellationToken);
        Task LogoutUserAsync(string? accessToken);
        Task ConfirmUserEmailAsync(EmailConfirmationRequest confirmationRequest);
        Task<RefreshTokenResponse> RefreshAccessTokenAsync(
            string? accessToken,
            RefreshTokenRequest tokenRefreshRequest,
            CancellationToken cancellationToken);
        Task RemoveRefreshTokenAsync(TUser user);
    }
}
