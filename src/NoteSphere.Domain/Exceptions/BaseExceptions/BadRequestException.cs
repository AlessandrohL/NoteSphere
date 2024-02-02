using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BaseExceptions
{
    public abstract class BadRequestException : Exception
    {
        public List<string> Errors { get; }

        public BadRequestException(string message)
            : base(message)
        {
            Errors = new() { message };
        }

        public BadRequestException(List<string> errors)
            : base()
        {
            Errors = errors;
        }
    }
}
