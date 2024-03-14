using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class UnconfirmedUserEmailException : UnauthorizedException
    {
        public UnconfirmedUserEmailException()
            : base(key: "User", new string[]
            {
                "The user's email address has not yet been confirmed."
            })
        { }
    }
}
