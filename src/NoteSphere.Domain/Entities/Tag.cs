using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Tag : BaseEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Guid AppUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<NoteTag>? Notes { get; set; }

    }
}
