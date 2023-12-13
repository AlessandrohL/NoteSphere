using Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class NoteNotFoundException : NotFoundException
    {
        public NoteNotFoundException(Guid id)
            : base($"Note with ID {id} not found.")
        { }
    }
}
