using Domain.Abstractions;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Note : BaseEntity<Guid>
    {
        public string? Title { get; set; }
        public string? Content { get; set; }

    }
}
