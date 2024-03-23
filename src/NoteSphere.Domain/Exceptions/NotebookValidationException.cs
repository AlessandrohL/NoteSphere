using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class NotebookValidationException : BadRequestException
    {
        public NotebookValidationException(IEnumerable<string> errors)
            : base(key: "Notebook", errors)
        { }
    }
}
