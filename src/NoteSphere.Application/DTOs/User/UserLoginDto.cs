using Application.Errors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User
{
    public record UserLoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = AuthErrorsValidation.InvalidEmail)]
        public string? Email { get; init; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; init; }
    }
}
