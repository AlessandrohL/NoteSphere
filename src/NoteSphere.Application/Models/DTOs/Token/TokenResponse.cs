using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.Token
{
    public record TokenResponse
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }

        public TokenResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
