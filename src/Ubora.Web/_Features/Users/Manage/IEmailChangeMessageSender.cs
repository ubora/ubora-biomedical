using System.Threading.Tasks;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Users.Manage
{
    public interface IEmailChangeMessageSender
    {
        Task SendChangedEmailMessage(ApplicationUser user, string oldEmail);
        Task SendEmailChangeConfirmationMessage(ApplicationUser user, string newEmail);
    }
}
