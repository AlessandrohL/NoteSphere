using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BaseExceptions
{
    public abstract class ValidationException : Exception
    {
        public List<string> Errors { get; set; }

        public ValidationException(List<string> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
