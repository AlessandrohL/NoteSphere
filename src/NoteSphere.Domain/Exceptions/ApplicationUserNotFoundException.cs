using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class ApplicationUserNotFoundException : NotFoundException
    {
        public ApplicationUserNotFoundException()
            : base("The user with the provided Id was not found.")
        { }
    }
}
