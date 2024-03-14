using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class UserAlreadyConfirmedException : BadRequestException
    {
        public UserAlreadyConfirmedException()
            : base(key: "User", new string[]
            {
                "Your account has already been confirmed"
            })
        { }
    }
}
