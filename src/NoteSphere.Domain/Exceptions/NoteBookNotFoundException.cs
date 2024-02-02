using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class NotebookNotFoundException : NotFoundException
    {
        public NotebookNotFoundException(Guid id)
            : base($"The Notebook with Id '{id}' was not found")
        { }
    }
}
