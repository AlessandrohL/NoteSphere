using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Notebook : BaseEntity, ISoftDelete
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid AppUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Note>? Notes { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteAt { get; set; }

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
