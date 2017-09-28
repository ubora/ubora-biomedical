using System.Collections.Generic;
using System.Threading.Tasks;
using Ubora.Web.Infrastructure;

namespace Ubora.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, IEnumerable<EmailIcon> emailIcons = null);
    }
}
