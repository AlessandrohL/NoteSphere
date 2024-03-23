using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Notebook
{
    public sealed class PatchNotebookValidator : AbstractValidator<Domain.Entities.Notebook>
    {
        public PatchNotebookValidator()
        {
            RuleFor(n => n.Title)
                .MinimumLength(1)
                .NotEmpty();

            RuleFor(n => n.Description)
                .MaximumLength(70)
                .When(n => n.Description != null);
        }
    }
}
