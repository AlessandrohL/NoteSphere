using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BaseExceptions
{
    public abstract class NotFoundException : Exception
    {
        public List<string> Errors { get; }

        public NotFoundException(string message)
            : base(message)
        {
            Errors = new() { message };
        }

        public NotFoundException(List<string> errors)
            : base()
        {
            Errors = errors;
        }
    }
}
