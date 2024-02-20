using Application.Abstractions;
using Application.Common;
using Application.Errors;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Exceptions;
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

        public async Task<Guid> GetUserIdByTenantAsync(Guid tenantId)
        {
            var appUserId = await _unitOfWork.ApplicationUser
                .FindUserIdByTenantAsync(tenantId);

            if (appUserId == Guid.Empty)
            {
                throw new ApplicationUserNotFoundException();
            }

            return appUserId;
        }
    }
}
