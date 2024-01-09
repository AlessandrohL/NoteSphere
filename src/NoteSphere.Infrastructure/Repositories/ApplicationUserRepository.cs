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
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser, Guid>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context)
            : base(context)
        { }

        public async Task<ApplicationUser?> FindUserByIdentityIdAsync(string id, bool trackChanges)
        {
            return await FindByCondition(u => u.IdentityId == id, trackChanges)
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> FindUserIdByIdentityIdAsync(string id)
        {
            return await FindByCondition(u => u.IdentityId == id, false)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserExistsByIdentityAsync(string id)
        {
            return await FindByCondition(u => u.IdentityId == id, false)
                .AnyAsync();
        }
    }
}
