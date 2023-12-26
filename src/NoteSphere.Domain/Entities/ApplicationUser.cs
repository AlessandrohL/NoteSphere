using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class ApplicationUser : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? FirstNames { get; set; }
        public string? LastNames { get; set; }
        public string? ProfilePicture { get; set; }
        public string IdentityGuid { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<NoteBook>? NoteBooks { get; set; }
        public ICollection<Todo>? Todos { get; set; }
    }
}
