﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BaseExceptions
{
    public abstract class UnauthorizedException : Exception
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; init; }

        public UnauthorizedException(string key, IEnumerable<string> errors)
            : base()
        {
            Errors = new() { { key, errors } };
        }
    }
}
