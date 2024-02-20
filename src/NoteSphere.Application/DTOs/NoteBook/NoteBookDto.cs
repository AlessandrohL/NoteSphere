using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Notebook
{
    public record NotebookDto
    {
        public Guid Id { get; init; }
        public string? Title { get; init; }
        public string? Description { get; init; }
    }
}
