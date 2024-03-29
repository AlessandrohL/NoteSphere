﻿using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IApplicationUserService
    {
        Task<Guid> GetUserIdByTenantAsync(Guid tenantId, CancellationToken cancellationToken);
    }
}
