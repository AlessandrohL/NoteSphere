using Application.Abstractions;
using Infrastructure.Helpers;
using Microsoft.IdentityModel.Tokens;
using WebApi.Constants;

namespace WebApi.ContextAcessor
{
    public sealed class TenantProvider : ITenantProvider
    {
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtConfigHelper _jwtConfigHelper;
        private const string TenantClaimType = "tenantId";

        public TenantProvider(
            IJwtService jwtService, 
            IHttpContextAccessor httpContextAccessor, 
            JwtConfigHelper jwtConfigHelper)
        {
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _jwtConfigHelper = jwtConfigHelper;
        }

        public Guid GetTenantId()
        {
            var cookieName = _jwtConfigHelper.GetCookieName();
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null || httpContext.Request.Cookies[cookieName!]
                .IsNullOrEmpty())
            {
                return Guid.Empty;
            }

            string accessToken = httpContext
                .Request
                .Cookies[cookieName!]!;

            var claims = _jwtService.GetClaimsFromToken(accessToken)?.ToList();
            var tenantClaim = claims?.FirstOrDefault(c => c.Type == TenantClaimType);

            if (tenantClaim == null || string.IsNullOrEmpty(tenantClaim.Value)
                || !Guid.TryParse(tenantClaim.Value, out Guid tenantId))
            {
                throw new Exception("Tenant ID claim is not present in the access token.");
            }

            return tenantId;
        }

    }
}
