using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using Ubora.Web.Infrastructure;

namespace Ubora.Web.Services
{
    public class SpecifiedPickupDirectoryEmailSender : IEmailSender
    {
        private readonly IOptions<SmtpSettings> _appSettings;

        public SpecifiedPickupDirectoryEmailSender(IOptions<SmtpSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string message, IEnumerable<EmailIcon> emailIcons)
        {
            await SendEmailToPickupDirectory(email, subject, message, emailIcons);
        }

        private async Task SendEmailToPickupDirectory(string email, string subject, string message, IEnumerable<EmailIcon> emailIcons)
        {
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

            var pickupDirectory = _appSettings.Value.EmailPickupDirectory;

            if (!Directory.Exists(pickupDirectory))
                Directory.CreateDirectory(pickupDirectory);

            var directoryPath = Path.GetFullPath(Path.Combine(pickupDirectory, Guid.NewGuid() + ".eml"));

            using (var stream = new FileStream(directoryPath, FileMode.CreateNew))
            {
                emailMessage.WriteTo(stream);
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
