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
            : base(key: "AccessToken", new string[] 
            { 
                "The access token provided is invalid." 
            })
        { }

        public InvalidAccessTokenException(string message)
            : base(key: "AccessToken", new string[] { message })
        { }
    }
}
