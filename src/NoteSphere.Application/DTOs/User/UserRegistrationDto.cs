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
        public string? FirstNames { get; init; }
        public string? LastNames { get; init; }
        public string? UserName { get; init; }
        public string? Email { get; init; }
        public string? Password { get; init; }
    }
}
