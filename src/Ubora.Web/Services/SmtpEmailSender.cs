using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Ubora.Web.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IOptions<SmtpSettings> _appSettings;

        public SmtpEmailSender(IOptions<SmtpSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpUsername = _appSettings.Value.SmtpUsername;
            var smtpPassword = _appSettings.Value.SmtpPassword;
            var smtpHostname = _appSettings.Value.SmtpHostname;
            var smtpPort = _appSettings.Value.SmtpPort;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Ubora", smtpUsername));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpHostname, smtpPort, SecureSocketOptions.StartTlsWhenAvailable);
                client.AuthenticationMechanisms.Remove("XOAUTH2"); // Must be removed for Gmail SMTP
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
