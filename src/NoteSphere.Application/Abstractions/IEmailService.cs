using Application.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(Message message);
        string GenerateEmailConfirmationUrl(string userId, string token);
        string CreateEmailConfirmationTemplate(string firstName, string confirmationLink);
    }
}
