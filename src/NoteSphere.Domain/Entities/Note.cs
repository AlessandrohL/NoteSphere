using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Note : BaseEntity
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid NoteBookId { get; set; }
        public NoteBook? NoteBook { get; set; }
        public ICollection<NoteTag>? Tags { get; set; }
    }
}
