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
       Task<ApplicationUser?> FindUserByIdentityIdAsync(string identityId, bool trackChanges);
       Task<Guid> FindUserIdByIdentityIdAsync(string identityId);
       Task<bool> IsUserExistsByIdentityAsync(string identityId);

    }
}
