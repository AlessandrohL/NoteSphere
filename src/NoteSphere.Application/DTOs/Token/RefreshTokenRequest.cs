using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Token
{
    public record RefreshTokenRequest
    {
        public string? RefreshToken { get; init; }
    }
}
