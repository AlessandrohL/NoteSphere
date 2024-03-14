using Application.DTOs.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public sealed class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        private const string EmailRegex = @"^\S+@\S+$";

        public UserLoginValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .Matches(EmailRegex)
                    .WithMessage("The email entered is not valid.");

            RuleFor(u => u.Password)
                .NotEmpty();
        }
    }
}
