using Domain.Abstractions;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Todo : BaseEntity, ITenantEntity
    {
        public int Id { get; set; }
        public Guid TenantId { get; private set; }
        public string? Title { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.None;
        public bool IsComplete { get; set; } = false;

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
