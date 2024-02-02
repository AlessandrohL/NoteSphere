using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BaseExceptions
{
    public abstract class UnauthorizedException : Exception
    {
        public List<string> Errors { get; set; }

        public UnauthorizedException(string message)
            : base(message)
        {
            Errors = new() { message };
        }

        public UnauthorizedException(List<string> errors)
            : base()
        {
            Errors = errors;
        }
    }
}
