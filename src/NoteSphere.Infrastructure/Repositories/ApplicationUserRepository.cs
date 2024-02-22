using Domain.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ApplicationUserRepository : 
        RepositoryBase<ApplicationUser, Guid>, 
        IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context)
            : base(context)
        { }

        public async Task<ApplicationUser?> FindUserByTenantAsync(
            Guid tenantId, 
            bool trackChanges,
            CancellationToken cancellationToken = default)
        {
            return await FindByCondition(u => u.TenantId == tenantId, trackChanges)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Guid> FindUserIdByTenantAsync(
            Guid tenantId, 
            CancellationToken cancellationToken = default)
        {
            return await FindByCondition(u => u.TenantId == tenantId, false)
                .Select(u => u.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsUserExistsByTenantAsync(
            Guid tenantId, 
            CancellationToken cancellationToken = default)
        {
            return await FindByCondition(u => u.TenantId == tenantId, false)
                .AnyAsync(cancellationToken);
        }
    }
}
