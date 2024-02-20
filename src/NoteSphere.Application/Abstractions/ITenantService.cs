using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface ITenantService
    {
        Task<Claim?> GetTenantFromUser(IUserWithIdentityFeatures user);
        List<Claim> GenerateClaimsForUser(
            IUserWithIdentityFeatures user, 
            Guid tenantId);
        Guid CreateTenant();
    }
}
