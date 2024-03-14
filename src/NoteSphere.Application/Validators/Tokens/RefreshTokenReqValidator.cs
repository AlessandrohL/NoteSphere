using Application.DTOs.Token;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Tokens
{
    public sealed class RefreshTokenReqValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenReqValidator()
        {
            RuleFor(t => t.RefreshToken)
                .NotEmpty();
        }
    }
}
