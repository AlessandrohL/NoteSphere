using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User
{
    public record UserRegistrationDto
    {
        [Required(ErrorMessage = "The name is required.")]
        [MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        public string? FirstNames { get; init; }

        [MaxLength(100, ErrorMessage = "Last names cannot exceed 100 characters.")]
        public string? LastNames { get; init; }

        [Required(ErrorMessage = "Username is required.")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }
    }
}
