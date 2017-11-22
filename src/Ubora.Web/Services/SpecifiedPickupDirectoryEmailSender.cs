using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Ubora.Web.Services
{
    public class SpecifiedPickupDirectoryEmailSender : EmailSender
    {
        private readonly IOptions<SmtpSettings> _appSettings;

        public SpecifiedPickupDirectoryEmailSender(IOptions<SmtpSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public override Task SendEmailAsync(string email, string subject, string message, Action<AttachmentCollection> handleAttachments = null, Action<AttachmentCollection> handleLinkedResources = null)
        {
            var emailMessage = PrepareEmailMessage(email, subject, message, handleAttachments, handleLinkedResources);

            var pickupDirectory = _appSettings.Value.EmailPickupDirectory;
            if (!Directory.Exists(pickupDirectory))
            {
                Directory.CreateDirectory(pickupDirectory);
            }

            var emailPath = Path.GetFullPath(Path.Combine(pickupDirectory, Guid.NewGuid() + ".eml"));
            using (var stream = new FileStream(emailPath, FileMode.CreateNew))
            {
                emailMessage.WriteTo(stream);
            }

            return Task.CompletedTask;
        }
    }
}