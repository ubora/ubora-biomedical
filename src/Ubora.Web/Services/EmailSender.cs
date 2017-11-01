using System;
using System.Threading.Tasks;
using MimeKit;

namespace Ubora.Web.Services
{
    public abstract class EmailSender
    {
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="handleAttachments"><see cref="BodyBuilder.Attachments"/></param>
        /// <param name="handleLinkedResources"><see cref="BodyBuilder.LinkedResources"/></param>
        public abstract Task SendEmailAsync(string email, string subject, string message, Action<AttachmentCollection> handleAttachments = null, Action<AttachmentCollection> handleLinkedResources = null);

        protected MimeMessage PrepareEmailMessage(string email, string subject, string message, Action<AttachmentCollection> handleAttachments, Action<AttachmentCollection> handleLinkedResources)
        {
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };

            handleAttachments?.Invoke(bodyBuilder.Attachments);
            handleLinkedResources?.Invoke(bodyBuilder.LinkedResources);

            var emailMessage = new MimeMessage
            {
                Subject = subject,
                Body = bodyBuilder.ToMessageBody()
            };

            emailMessage.From.Add(new MailboxAddress("Ubora", "noreply@ubora-biomedical.org"));
            emailMessage.To.Add(new MailboxAddress("", email));

            return emailMessage;
        }
    }
}