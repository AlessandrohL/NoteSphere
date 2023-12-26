using Application.Abstractions;
using Application.Models.DTOs.Token;
using Application.Models.DTOs.User;
using Application.Models.Identity;
using Microsoft.AspNetCore.Mvc;

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
            if (userRegistration is null) return BadRequest();

            var result = await _authenticationService.RegisterUserAsync(userRegistration);

            return result.Match<IActionResult>(
                success => StatusCode(StatusCodes.Status201Created, success), BadRequest);
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLogin)
        {
            var result = await _authenticationService.LoginUserAsync(userLogin);

            return result.Match<IActionResult>(Ok, Unauthorized);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshRequest tokenRefresh)
        {
            var result = await _authenticationService.RefreshTokenAsync(tokenRefresh);

            return result.Match<IActionResult>(Ok, Unauthorized);
        }
    }
}
