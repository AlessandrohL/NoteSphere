using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors
{
    public static class JwtErrorMessages
    {
        public const string InvalidAccessToken = "The provided token is invalid.";
        public const string InvalidRefreshToken = "The provided refresh token is invalid.";
        public const string TokenExpired = "The token has expired.";
        public const string RefreshTokenExpired = "The refresh token has expired. Please log in again to obtain a new token.";
    }
}
