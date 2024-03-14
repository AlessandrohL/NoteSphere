using Application.Abstractions;
using Application.Email;
using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly ClientConfig _clientConfig;
        private readonly IUrlUtility _urlUtility;

        public EmailService(
            IEmailSender emailSender, 
            ClientConfig clientConfig, 
            IUrlUtility urlUtility)
        {
            _emailSender = emailSender;
            _clientConfig = clientConfig;
            _urlUtility = urlUtility;
        }

        public string GenerateEmailConfirmationUrl(string userId, string token)
        {
            var clientConfirmationUrl = $"{_clientConfig.BaseUrl}/{_clientConfig.ConfirmationEndpoint}";
            var encodedToken = _urlUtility.EncodeUrlToBase64(token);
            var confirmationLink = $"{clientConfirmationUrl}?id={userId}&code={encodedToken}";
            
            return confirmationLink;
        }

        public string CreateEmailConfirmationTemplate(string firstName, string confirmationLink)
        {
            string templatePath = GetEmailComfirmationTemplatePath();
            
            string templateContent = File.ReadAllText(templatePath);
            templateContent = templateContent.Replace("{userFirstName}", firstName);
            templateContent = templateContent.Replace("{confirmationLink}", confirmationLink);
            
            return templateContent;
        }

        private static string GetEmailComfirmationTemplatePath()
        {
            var dirPath = Directory.GetCurrentDirectory() + "\\Templates\\";
            var templatePath = dirPath + "EmailConfirmation.html";

            return templatePath;
        }

        public Task SendEmailConfirmationAsync(Message message)
            => _emailSender.SendEmailAsync(message);

    }
}
