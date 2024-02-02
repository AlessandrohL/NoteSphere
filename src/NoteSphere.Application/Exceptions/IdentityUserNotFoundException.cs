using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class IdentityUserNotFoundException : NotFoundException
    {
        public IdentityUserNotFoundException()
            :base("Account not found. Please register or verify your credentials.")
        { }
    }
}
