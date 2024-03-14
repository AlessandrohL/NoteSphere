using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class UserAlreadyExistsException : UnprocessableException
    {
        public UserAlreadyExistsException(IEnumerable<string> errors)
            : base("User", errors)
        { }
    }
}
