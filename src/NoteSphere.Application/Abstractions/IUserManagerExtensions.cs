using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IUserManagerExtensions<TUser> where TUser : IUserWithIdentityFeatures
    {
        Task<bool> IsEmailAlreadyInUseAsync(string email, CancellationToken cancellationToken);
        Task<bool> IsUsernameAlreadyInUseAsync(string username, CancellationToken cancellationToken);

    }
}
