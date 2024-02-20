using Application.Abstractions;
using Application.DTOs.Token;
using Application.DTOs.User;
using Application.Identity;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using WebApi.ActionFilters;
using WebApi.Common;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService<UserAuth> _authenticationService;

        public AuthController(IAuthenticationService<UserAuth> authenticationService)
            => _authenticationService = authenticationService;


        [HttpPost("register")]
        [RemoveAuthorizationHeader]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var headers = HttpContext.Request.Headers;

            if (userRegistration is null)
            {
                return BadRequest();
            }

            var tokenResponse = await _authenticationService.RegisterUserAsync(userRegistration);

            return Ok(SuccessResponse<TokenResponse>.Ok(tokenResponse));
        }

        [HttpPost("login")]
        [RemoveAuthorizationHeader]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLogin)
        {
            var headers = HttpContext.Request.Headers;

            var tokenResponse = await _authenticationService.LoginUserAsync(userLogin);

            return Ok(SuccessResponse<TokenResponse>.Ok(tokenResponse));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshRequest tokenRefresh)
        {
            var tokenResponse = await _authenticationService.RefreshTokenAsync(tokenRefresh);

            return Ok(SuccessResponse<TokenResponse>.Ok(tokenResponse));
        }
    }
}
