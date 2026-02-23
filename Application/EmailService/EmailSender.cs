using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HR Team", mail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(host, port, true);
            await smtpClient.AuthenticateAsync(mail, password);
            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);
        }
    }
}
