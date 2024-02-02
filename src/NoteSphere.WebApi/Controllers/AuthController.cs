using Application.Abstractions;
using Application.DTOs.Token;
using Application.DTOs.User;
using Application.Identity;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            if (userRegistration is null)
            {
                return BadRequest();
            }

            var tokenResponse = await _authenticationService.RegisterUserAsync(userRegistration);

            return Ok(SuccessResponse<TokenResponse>.Ok(tokenResponse));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLogin)
        {
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
