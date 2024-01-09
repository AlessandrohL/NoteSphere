using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Notes
{
    public record NoteDto
    {
        public Guid Id { get; init; }
    }
}
