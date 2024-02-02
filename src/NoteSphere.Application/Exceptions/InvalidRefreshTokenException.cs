using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class InvalidRefreshTokenException
        : BadRequestException
    {
        public InvalidRefreshTokenException()
            : base("The refresh token provided is invalid.")
        { }
    }
}
