using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using System.Collections.Generic;
using Ubora.Web.Infrastructure;

namespace Ubora.Web.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IOptions<SmtpSettings> _appSettings;

        public SmtpEmailSender(IOptions<SmtpSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string message, IEnumerable<EmailIcon> emailIcons)
        {
            var smtpUsername = _appSettings.Value.SmtpUsername;
            var smtpPassword = _appSettings.Value.SmtpPassword;
            var smtpHostname = _appSettings.Value.SmtpHostname;
            var smtpPort = _appSettings.Value.SmtpPort;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Ubora", "noreply@ubora-biomedical.org"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var builder = new BodyBuilder();
            if(emailIcons != null)
            {
                AddIconsToEmail(emailIcons, builder);
            }

            builder.HtmlBody = message;
            emailMessage.Body = builder.ToMessageBody();

            var otherClient = new SmtpClient();
            otherClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(smtpHostname, smtpPort, SecureSocketOptions.StartTlsWhenAvailable);
                client.AuthenticationMechanisms.Remove("XOAUTH2"); // Must be removed for Gmail SMTP
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        private static void AddIconsToEmail(IEnumerable<EmailIcon> emailIcons, BodyBuilder builder)
        {
            foreach (var emailIcon in emailIcons)
            {
                var image = builder.LinkedResources.Add(emailIcon.Path);
                image.ContentId = emailIcon.ContentId;
            }
        }
    }
}
