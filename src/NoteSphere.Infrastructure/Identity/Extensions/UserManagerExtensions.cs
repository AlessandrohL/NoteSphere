using Application.Abstractions;
using Application.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Extensions
{
    public sealed class UserManagerExtensions : IUserManagerExtensions<UserAuth>
    {
        private readonly UserManager<UserAuth> _userManager;

        public UserManagerExtensions(UserManager<UserAuth> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsEmailAlreadyInUseAsync(string email, CancellationToken cancellationToken)
        {
            return await _userManager.Users
                .AnyAsync(u => u.Email == email, cancellationToken);    
        }

        public async Task<bool> IsUsernameAlreadyInUseAsync(string username, CancellationToken cancellationToken)
        {
            return await _userManager.Users
                .AnyAsync(u => u.UserName == username, cancellationToken);
        }
    }
}
