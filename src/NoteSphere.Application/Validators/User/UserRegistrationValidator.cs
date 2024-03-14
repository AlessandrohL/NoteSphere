using Application.DTOs.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public sealed class UserRegistrationValidator : AbstractValidator<UserRegistrationDto>
    {

        public UserRegistrationValidator()
        {
            RuleFor(u => u.FirstNames)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Matches(@"^[^\s\d][a-zA-Z\s]*$")
                    .WithMessage("Name must not start with a space or a number, and can only contain letters and spaces.")
                .MaximumLength(40)
                .MinimumLength(2);

            When(u => string.IsNullOrEmpty(u.LastNames), () =>
            {
                RuleFor(u => u.LastNames)
                 .Cascade(CascadeMode.Stop)
                 .Matches(@"^[^\s\d][a-zA-Z\s]*$")
                    .WithMessage("Last name must not start with a space or a number, and can only contain letters and spaces.")
                 .MaximumLength(40)
                 .MinimumLength(2);
            });

            RuleFor(u => u.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(18)
                .Matches(@"^[a-zA-Z0-9](_(?!(\.|_))|\.(?!(_|\.))|[a-zA-Z0-9]){4,18}[a-zA-Z0-9]$")
                    .WithMessage("Username must contain alphanumeric characters and may include underscores or periods in the middle, but not at the beginning or end. There cannot be two consecutive underscores or colons.");

            RuleFor(u => u.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Matches(@"^\S.*@\S+$")
                    .WithMessage("The provided email address is not valid. Please ensure it is in the correct format (for example, user@example.com).");

            RuleFor(u => u.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(8)
                .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).+$")
                    .WithMessage("The password must contain at least one capital letter, one digit and one non-alphanumeric character.");

        }
    }
}
