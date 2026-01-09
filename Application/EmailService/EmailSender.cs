using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SenEmailAsync(string email, string subject, string body)
        {
            var mail = _configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = _configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Gmail email or App Password is missing.");

            using var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, password)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(mail, "HR Team"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            message.To.Add(email);

            await smtpClient.SendMailAsync(message);
        }

    }


}

