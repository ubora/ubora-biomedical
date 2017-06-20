using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Ubora.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IOptions<AppSettings> _appSettings;

        public EmailSender(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        //Configuration for can add here to sending email via SMTP or Api
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await Task.Run(() => SendEmailToPickupDirectory(email, subject, message));
        }

        private void SendEmailToPickupDirectory(string email, string subject, string message)
        {
            var pickupDirectory = _appSettings.Value.EmailPickupDirectory;

            if (!Directory.Exists(pickupDirectory))
                Directory.CreateDirectory(pickupDirectory);

            var path = Path.GetFullPath(Path.Combine(pickupDirectory, Guid.NewGuid() + ".eml"));
            var stream = new FileStream(path, FileMode.CreateNew);
            var writer = new StreamWriter(stream);
            writer.WriteLine(DateTime.Now);
            writer.WriteLine("Subject: " + subject);
            writer.WriteLine("To: " + email);
            writer.WriteLine("");
            writer.WriteLine(message);
            writer.Dispose();
        }
    }
}
