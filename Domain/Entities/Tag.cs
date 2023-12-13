using Domain.Abstractions;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Tag : BaseEntity<int>
    {
        public string? Name { get; set; }
        public ICollection<NoteTag>? NoteTags { get; set; }
    }
}
