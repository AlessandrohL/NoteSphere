using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IApplicationUserService
    {
        Task<Result<Guid, string>> GetUserIdByIdentityIdAsync(string identityId);
    }
}
