using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class ApplicationUser : BaseEntity
    {
        public Guid Id { get; set; }
        public string? FirstNames { get; set; }
        public string? LastNames { get; set; }
        public string? ProfilePicture { get; set; }
        public string IdentityId { get; set; } = null!;
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<Notebook>? Notebooks { get; set; }

        public void AssignIdentity(string id) => IdentityId = id;
    }
}
