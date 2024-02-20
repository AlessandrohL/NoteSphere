using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions.Repositories
{
    public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser, Guid>
    {
        Task<ApplicationUser?> FindUserByTenantAsync(Guid tenantId, bool trackChanges);
        Task<Guid> FindUserIdByTenantAsync(Guid tenantId);
        Task<bool> IsUserExistsByTenantAsync(Guid tenantId);

    }
}
