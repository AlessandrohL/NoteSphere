using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors
{
    public static class AuthErrorMessages
    {
        public const string InvalidEmail = "The email address is not valid";
        public const string InvalidCredentials = "Invalid email or password. Please check your credentials and try again.";
        public const string UserNotFound = "Account not found. Please register or verify your email.";
    
    }
}
