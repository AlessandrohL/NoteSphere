using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class RefreshTokenExpiredException
        : BadRequestException
    {
        public RefreshTokenExpiredException()
            : base("The refresh token has expired. Please log in again to obtain a new token.")
        { }
    }
}
