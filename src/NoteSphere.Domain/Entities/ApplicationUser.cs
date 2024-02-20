using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class ApplicationUser : BaseEntity, ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; private set; }
        public string? FirstNames { get; set; }
        public string? LastNames { get; set; }
        public string? ProfilePicture { get; set; }

        public void AssignTenant(Guid tenantId)
        {
            if (!Guid.TryParse(TenantId.ToString(), out _)
                || tenantId == Guid.Empty)
            {
                throw new Exception("The tenantID provided is not a valid GUID or is an empty GUID.");
            }

            TenantId = tenantId;
        }
    }
}
