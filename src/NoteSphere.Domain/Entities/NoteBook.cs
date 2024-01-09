using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class NoteBook : BaseEntity
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid AppUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Note>? Notes { get; set; }

    }
}
