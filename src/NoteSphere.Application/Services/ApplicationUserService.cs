using Application.Abstractions;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
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
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserService(
            IUnitOfWork unitOfWork,
            IApplicationUserRepository applicationUserRepository)
        {
            _unitOfWork = unitOfWork;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<Guid> GetUserIdByTenantAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            var appUserId = await _applicationUserRepository
                .FindUserIdByTenantAsync(tenantId, cancellationToken);

            if (appUserId == Guid.Empty)
            {
                throw new ApplicationUserNotFoundException();
            }

            return appUserId;
        }
    }
}
