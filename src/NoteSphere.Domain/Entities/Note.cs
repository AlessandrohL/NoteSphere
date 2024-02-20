using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Note : BaseEntity, ISoftDeleteEntity
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid NotebookId { get; set; }
        public Notebook? Notebook { get; set; }
        public ICollection<NoteTag>? Tags { get; set; }
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
