using Application.DTOs.Notes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Note
{
    public sealed class CreateNoteValidator : AbstractValidator<CreateNoteDto>
    {
        public CreateNoteValidator()
        {
            RuleFor(n => n.Title)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(60);
        }
    }
}
