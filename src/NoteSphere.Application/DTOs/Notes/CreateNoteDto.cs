using Domain.Errors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Notes
{
    public record CreateNoteDto
    {
        [Required(ErrorMessage = NoteErrorsValidation.TitleRequired)]
        [MinLength(1, ErrorMessage = "The note title must be at least 1 character long.")]
        [MaxLength(60, ErrorMessage = "The note title must not exceed 60 characters.")]
        public string? Title { get; init; }
    }
}
