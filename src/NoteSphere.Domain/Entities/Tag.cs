using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Tag : BaseEntity, ISoftDeleteEntity, ITenantEntity
    {
        public int Id { get; set; }
        public Guid TenantId { get; private set; }
        public string? Name { get; set; }
        public ICollection<NoteTag>? Notes { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteAt { get; set; }

        public void AssignTenant(Guid tenantId)
        {
            if (!Guid.TryParse(TenantId.ToString(), out _)
                || tenantId == Guid.Empty)
            {
                throw new Exception("The tenantID provided is not a valid GUID or is an empty GUID.");
            }

            TenantId = tenantId;
        }

        public void Delete()
        {
            IsDeleted = true;
            DeleteAt = DateTime.UtcNow;
            SetModified();
        }
        public void Restore()
        {
            IsDeleted = default;
            DeleteAt = null;
            SetModified();
        }
    }
}
