using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.Token
{
    public record TokenRefreshRequest
    {
        [Required(ErrorMessage = "Access token is required.")]
        public string? AccessToken { get; init; }

        [Required(ErrorMessage = "A refresh token is required.")]
        public string? RefreshToken { get; init; }
    }
}
