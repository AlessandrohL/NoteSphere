using Domain.Abstractions;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Todo : BaseEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.None;
        public bool IsComplete { get; set; } = false;
        public Guid AppUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
