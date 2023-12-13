using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class UserProfileNotFoundException : NotFoundException
    {
        public UserProfileNotFoundException(Guid id)
            : base($"User Profile with ID {id} not found.")
        { }
    }
}
