using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class InvalidAccessTokenException
        : BadRequestException
    {
        public InvalidAccessTokenException()
            : base("The access token provided is invalid.")
        { }
    }
}
