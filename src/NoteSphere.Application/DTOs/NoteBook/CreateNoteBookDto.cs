using Domain.Errors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.NoteBook
{
    public record CreateNoteBookDto
    {
        [Required(ErrorMessage = NotebookErrors.TitleNullOrEmpty)]
        [MinLength(1, ErrorMessage = NotebookErrors.TitleTooShort)]
        public string? Title { get; init; }

        [MaxLength(70, ErrorMessage = NotebookErrors.DescriptionMaxLength)]
        public string? Description { get; init; }
    }
}
