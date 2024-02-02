using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class IdentityUserInvalidCredentialsException : 
        UnauthorizedException
    {
        public IdentityUserInvalidCredentialsException()
            : base("Invalid email or password. Please check your credentials and try again.")
        { }
    }
}
