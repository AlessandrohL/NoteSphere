using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class RefreshTokenNotSetException : BadRequestException
    {
        public RefreshTokenNotSetException()
            : base(key: "RefreshToken", new string[] 
            { 
                "User does not have a refresh token set." 
            })
        { }
    }
}
