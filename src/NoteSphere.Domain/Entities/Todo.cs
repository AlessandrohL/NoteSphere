using Domain.Abstractions;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Todo : IBaseEntity<int>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.None;
        public bool IsComplete { get; set; } = false;
        public Guid AppUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
