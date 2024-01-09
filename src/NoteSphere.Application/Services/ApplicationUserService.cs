using Application.Abstractions;
using Application.Common;
using Domain.Abstractions;
using Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ApplicationUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, string>> GetUserIdByIdentityIdAsync(string identityId)
        {
            if (string.IsNullOrEmpty(identityId)) return "";

            var appUserId = await _unitOfWork.ApplicationUser.FindUserIdByIdentityIdAsync(identityId);
            
            if (appUserId == Guid.Empty) return ApplicationUserErrors.NotFound;

            return appUserId;
        }
    }
}
