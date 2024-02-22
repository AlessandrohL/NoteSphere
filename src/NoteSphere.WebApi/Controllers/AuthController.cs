using Application.Abstractions;
using Application.DTOs.Token;
using Application.DTOs.User;
using Application.Identity;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ActionFilters;
using WebApi.Common;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService<UserAuth> _authenticationService;

        public AuthController(IAuthenticationService<UserAuth> authenticationService)
            => _authenticationService = authenticationService;


        [HttpPost("register")]
        [RemoveAuthorizationHeader]
        public async Task<IActionResult> RegisterUser(
            [FromBody] UserRegistrationDto userRegistration,
            CancellationToken cancellationToken)
        {
            var tokenResponse = await _authenticationService
                .RegisterUserAsync(userRegistration, cancellationToken);

            return Ok(SuccessResponse<TokenResponse>.Ok(tokenResponse));
        }

        [HttpPost("login")]
        [RemoveAuthorizationHeader]
        public async Task<IActionResult> LoginUser(
            [FromBody] UserLoginDto userLogin,
            CancellationToken cancellationToken)
        {
            var tokenResponse = await _authenticationService
                .LoginUserAsync(userLogin, cancellationToken);

            return Ok(SuccessResponse<TokenResponse>.Ok(tokenResponse));
        }

        [HttpPost("refresh")]
        [RemoveAuthorizationHeader]
        public async Task<IActionResult> RefreshToken(
            [FromBody] TokenRefreshRequest tokenRefresh, 
            CancellationToken cancellationToken)
        {
            var tokenResponse = await _authenticationService
                .RefreshTokenAsync(tokenRefresh, cancellationToken);

            return Ok(SuccessResponse<TokenResponse>.Ok(tokenResponse));
        }
    }
}
