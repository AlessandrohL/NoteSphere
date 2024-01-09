using System.Security.Claims;

namespace WebApi.ContextAcessor
{
    public class UserContextAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContextAccessor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string? GetCurrentIdentityId()
        {
            return _contextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
