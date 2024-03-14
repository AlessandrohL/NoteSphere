using Application.Abstractions;
using Application.Email;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Email
{
    public sealed class EmailConfirmationRequestValidator 
        : AbstractValidator<EmailConfirmationRequest>
    {

        public EmailConfirmationRequestValidator(IUrlUtility urlUtility)
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .Must(x => Guid.TryParse(x, out _))
                    .WithMessage("Id is not valid.");

            RuleFor(e => e.Code)
                .NotEmpty()
                .Must(c => urlUtility.TryDecodeBase64Url(c!, out _))
                    .WithMessage("Code is not valid.");
        }
    }
}
