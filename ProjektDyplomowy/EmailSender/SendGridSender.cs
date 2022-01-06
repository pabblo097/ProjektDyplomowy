using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektDyplomowy.EmailSender
{
    public class SendGridSender : IEmailSender
    {
        private readonly SendGridSenderOption options;

        public SendGridSender(IOptions<SendGridSenderOption> options)
        {
            this.options = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("paw.wiewiora@protonmail.com", "ExamApp");
            var to = new EmailAddress(email, "New User");
            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            return client.SendEmailAsync(msg);
        }
    }
}