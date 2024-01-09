using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.NoteBook
{
    public record NoteBookDto
    {
        public Guid Id { get; init; }
        public string? Title { get; init; }
        public string? Description { get; init; }
    }
}
