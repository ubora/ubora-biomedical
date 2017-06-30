﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Ubora.Web.Services
{
    public class SpecifiedPickupDirectoryEmailSender : IEmailSender
    {
        private readonly IOptions<SmtpSettings> _appSettings;

        public SpecifiedPickupDirectoryEmailSender(IOptions<SmtpSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await SendEmailToPickupDirectory(email, subject, message);
        }

        private async Task SendEmailToPickupDirectory(string email, string subject, string message)
        {
            var pickupDirectory = _appSettings.Value.EmailPickupDirectory;

            if (!Directory.Exists(pickupDirectory))
                Directory.CreateDirectory(pickupDirectory);

            var path = Path.GetFullPath(Path.Combine(pickupDirectory, Guid.NewGuid() + ".eml"));
            var stream = new FileStream(path, FileMode.CreateNew);
            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteLineAsync(DateTime.Now.ToString());
                await writer.WriteLineAsync("Subject: " + subject);
                await writer.WriteLineAsync("To: " + email);
                await writer.WriteLineAsync("");
                await writer.WriteLineAsync(message);
            }
        }
    }
}