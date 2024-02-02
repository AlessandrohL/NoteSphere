using Domain.Errors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Notebook
{
    public record UpdateNotebookDto
    {
        [Required(ErrorMessage = NotebookErrorValidations.TitleNullOrEmpty)]
        [MinLength(1, ErrorMessage = NotebookErrorValidations.TitleTooShort)]
        public string? Title { get; init; }

        [MaxLength(70, ErrorMessage = NotebookErrorValidations.DescriptionMaxLength)]
        public string? Description { get; init; }
    }
}
