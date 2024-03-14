using Domain.Abstractions;
using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotebookNotDeletedException : BadRequestException
    {
        public NotebookNotDeletedException(Guid notebookId)
            : base(key: "Notebook", new string[]
            {
                $"The notebook with Id {notebookId} is not in the trash."
            })
        { }
    }
}
