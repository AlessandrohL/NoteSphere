using Domain.Abstractions;
using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Todo : BaseEntity<int>
    {
        public string? Title { get; set; }
        public PriorityLevel Priority { get; set; }
        public bool IsComplete { get; set; }
    }
}
