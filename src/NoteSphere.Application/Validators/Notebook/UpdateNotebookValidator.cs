using Application.DTOs.Notebook;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Notebook
{
    public sealed class UpdateNotebookValidator : AbstractValidator<UpdateNotebookDto>
    {
        public UpdateNotebookValidator()
        {
            RuleFor(n => n.Title)
                .NotEmpty()
                .MinimumLength(1);

            RuleFor(n => n.Description)
                .MaximumLength(70)
                .When(n => !string.IsNullOrEmpty(n.Description));
        }
    }
}
