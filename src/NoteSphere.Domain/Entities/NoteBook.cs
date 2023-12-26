using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class NoteBook : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public Guid AppUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Note>? Notes { get; set; } 
    }
}
