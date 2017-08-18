using System.Threading.Tasks;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Users.Account
{
    public interface IEmailConfirmationMessageSender
    {
        Task SendEmailConfirmationMessage(ApplicationUser user);
    }
}
