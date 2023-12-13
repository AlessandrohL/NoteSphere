using Domain.Abstractions;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class NoteBook : BaseEntity<Guid>
    {
        public string? Title { get; set; }
        public ICollection<Note>? Notes { get; set; }
    }
}
