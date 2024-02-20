using Application.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.ContextAcessor
{
    public sealed class TenantProvider : ITenantProvider
    {
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetTenantId()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null || httpContext.Request.Headers["Authorization"].IsNullOrEmpty())
            {
                return Guid.Empty;
            }

            string accessToken = _httpContextAccessor.HttpContext?
                .Request
                .Headers["Authorization"]!;

            var claims = _jwtService.ValidateToken(accessToken);
            var tenantClaim = claims.FindFirst("tenantId");

            if (tenantClaim == null || string.IsNullOrEmpty(tenantClaim.Value)
                || !Guid.TryParse(tenantClaim.Value, out Guid tenantId))
            {
                throw new Exception("Tenant ID claim is not present in the access token.");
            }

            return tenantId;
        }

    }
}
