using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class NoteBookNotFoundException : NotFoundException
    {
        public NoteBookNotFoundException(Guid id)
            : base($"Notebook with ID {id} not found.")
        { }
    }
}
