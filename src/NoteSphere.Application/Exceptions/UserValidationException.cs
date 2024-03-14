using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class UserValidationException
        : ValidationException
    {
        public UserValidationException(IEnumerable<string> errors) 
            : base(key: "User", errors)
        { }
    }
}
