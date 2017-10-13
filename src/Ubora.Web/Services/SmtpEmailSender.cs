using System;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Ubora.Web.Services
{
    public class SmtpEmailSender : EmailSender
    {
        private readonly IOptions<SmtpSettings> _appSettings;

        public SmtpEmailSender(IOptions<SmtpSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public override async Task SendEmailAsync(string email, string subject, string message, Action<AttachmentCollection> handleAttachments = null, Action<AttachmentCollection> handleLinkedResources = null)
        {
            var emailMessage = PrepareEmailMessage(email, subject, message, handleAttachments, handleLinkedResources);

            var smtpUsername = _appSettings.Value.SmtpUsername;
            var smtpPassword = _appSettings.Value.SmtpPassword;
            var smtpHostname = _appSettings.Value.SmtpHostname;
            var smtpPort = _appSettings.Value.SmtpPort;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
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