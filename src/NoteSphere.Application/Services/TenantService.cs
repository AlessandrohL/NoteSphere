using Application.Abstractions;
using Application.Helpers;
using Application.Identity;
using Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TenantService : ITenantService
    {
        public const string TenantClaimName = "tenantId";
        private readonly UserManager<UserAuth> _userManager;

        public TenantService(UserManager<UserAuth> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<Claim?> GetTenantFromUser(IUserWithIdentityFeatures user)
        {
            if (user is not UserAuth identityUser)
            {
                throw new ArgumentException("User is not a valid UserAuth");
            }

            var userClaims = await _userManager.GetClaimsAsync(identityUser!);

            if (userClaims == null || !userClaims.Any())
            {
                return null;
            }

            var userTenant = userClaims.FirstOrDefault(c => c.Type == TenantClaimName);

            return userTenant;
        }

        public List<Claim> GenerateClaimsForUser(IUserWithIdentityFeatures user, Guid tenantId)
        {
            return new List<Claim>()
            {
               new (JwtClaimTypes.Subject, user.Id),
               new (JwtClaimTypes.Name, user.UserName!),
               new (JwtClaimTypes.Email, user.Email!),
               new (TenantClaimName, tenantId.ToString())
            };
        }

        public Guid CreateTenant() => Guid.NewGuid();
    }

    public static class TenantExtensions
    {
        public static Claim ToClaim(this Guid tenantId)
            => new (TenantService.TenantClaimName, tenantId.ToString());
    }
}
