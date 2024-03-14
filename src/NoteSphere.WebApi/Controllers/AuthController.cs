using Application.Abstractions;
using Application.DTOs.Token;
using Application.DTOs.User;
using Application.Email;
using Application.Identity;
using FluentValidation;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<UserRegistrationDto> _registerValidator;
        private readonly IValidator<EmailConfirmationRequest> _emailConfValidator;
        private readonly IValidator<UserLoginDto> _loginValidor;
        private readonly IAuthenticationService<UserAuth> _authenticationService;
        private readonly JwtConfigHelper _jwtConfigHelper;
        private readonly IHttpContextAccessor _httpCxtAccessor;

        public AuthController(
            IValidator<UserRegistrationDto> registerValidator,
            IValidator<UserLoginDto> loginValidator, 
            IValidator<EmailConfirmationRequest> emailConfValidator,
            IAuthenticationService<UserAuth> authenticationService,
            IHttpContextAccessor httpCxtAccessor,
            JwtConfigHelper jwtConfigHelper)
        {
            _registerValidator = registerValidator;
            _loginValidor = loginValidator;
            _emailConfValidator = emailConfValidator;
            _authenticationService = authenticationService;
            _httpCxtAccessor = httpCxtAccessor;
            _jwtConfigHelper = jwtConfigHelper;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(
            [FromBody] UserRegistrationDto userRegistration,
            CancellationToken cancellationToken)
        {
            var result = _registerValidator.Validate(userRegistration);

            if (!result.IsValid)
            {
                result.AddValidationToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

            await _authenticationService.RegisterUserAsync(userRegistration, cancellationToken);

            return StatusCode(
                StatusCodes.Status201Created, 
                SuccessResponse<string>.Created("Your account has been successfully created. Please verify your email address to complete the registration process."));
        }

        [HttpPost("email-confirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmation(
          [FromBody] EmailConfirmationRequest request)
        {
            var result = _emailConfValidator.Validate(request);

            if (!result.IsValid)
            {
                result.AddValidationToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

            await _authenticationService.ConfirmUserEmailAsync(request);

            return Ok(SuccessResponse<string>.Ok("Successful confirmation!"));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(
            [FromBody] UserLoginDto userLogin,
            CancellationToken cancellationToken)
        {
            var result = _loginValidor.Validate(userLogin);

            if (!result.IsValid)
            {
                result.AddValidationToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

            var tokenResponse = await _authenticationService
                .LoginUserAsync(userLogin, cancellationToken);

            AppendAccessTokenInCookie(tokenResponse.AccessToken);

            return Ok(SuccessResponse<TokensResponse>.Ok(tokenResponse));
        }

        [HttpGet("authenticated")]
        [AllowAnonymous]
        public IActionResult IsAuthenticated()
        {
            return Ok(SuccessResponse<bool>.NoContent());
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            var accessToken = GetAccessTokenFromCookie();

            await _authenticationService.LogoutUserAsync(accessToken);

            RemoveCookieAccessToken();

            return Ok(SuccessResponse<bool>.NoContent());
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAccessToken(
            [FromBody] RefreshTokenRequest tokenRefresh,
            CancellationToken cancellationToken)
        {
            var accessToken = GetAccessTokenFromCookie();

            var tokenResponse = await _authenticationService
                .RefreshAccessTokenAsync(accessToken, tokenRefresh, cancellationToken);

            AppendAccessTokenInCookie(tokenResponse.RenewedAccessToken!);

            return Ok(SuccessResponse<RefreshTokenResponse>.Ok(tokenResponse));
        }

        private void AppendAccessTokenInCookie(string accessToken)
        {
            var cookieName = _jwtConfigHelper.GetCookieName();
            var cookieExpiration = _jwtConfigHelper.GetCookieExpirationDays();

            _httpCxtAccessor.HttpContext?.Response.Cookies.Append(
                key: cookieName!,
                value: accessToken,
                new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddDays(cookieExpiration),
                    HttpOnly = true,
                    Secure =  true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });
        }

        private string? GetAccessTokenFromCookie()
        {
            var cookieName = _jwtConfigHelper.GetCookieName();

            var accessToken = _httpCxtAccessor.HttpContext?
                .Request
                .Cookies[cookieName!];

            return accessToken;
        }

        private void RemoveCookieAccessToken()
        {
            var cookieName = _jwtConfigHelper.GetCookieName();

            _httpCxtAccessor.HttpContext?.Response.Cookies
                .Delete(cookieName!, new()
                {
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
        }
    }
}
