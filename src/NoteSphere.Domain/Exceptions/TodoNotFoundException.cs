using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class TodoNotFoundException : NotFoundException
    {
        public TodoNotFoundException(int id)
            : base($"To-do with ID {id} not found.")
        { }
    }
}
