using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class TagNotFoundException : NotFoundException
    {
        public TagNotFoundException(int id)
            : base($"The tag with Id '{id}' was not found")
        { }
    }
}
