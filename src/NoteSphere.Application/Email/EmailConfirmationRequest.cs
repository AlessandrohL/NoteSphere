using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Email
{
    public record EmailConfirmationRequest
    {
        public string? Id { get; init; }
        public string? Code { get; init; }
    }
}
