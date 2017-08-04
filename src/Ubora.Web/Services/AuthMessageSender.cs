using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web.Data;

namespace Ubora.Web.Services
{
    public interface IAuthMessageSender
    {
        Task SendForgotPasswordMessageAsync(ApplicationUser user);
    }

    public class AuthMessageSender : IAuthMessageSender
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUrlHelper _urlHelper;
        private readonly IEmailSender _emailSender;

        public AuthMessageSender(UserManager<ApplicationUser> userManager, IUrlHelper urlHelper, IEmailSender emailSender)
        {
            _userManager = userManager;
            _urlHelper = urlHelper;
            _emailSender = emailSender;
        }

        public async Task SendForgotPasswordMessageAsync(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = _urlHelper.Action("ResetPassword", "Account", new { userId = user.Id, code }, 
                protocol: _urlHelper.ActionContext.HttpContext.Request.Scheme);

            var message = "<h1 style='color:#4777BB;'>Password reset</h1><p>You can reset your password by clicking <a href=\"" + callbackUrl + "\">this link</a>.</p>";

            await _emailSender.SendEmailAsync(user.Email, "Password reset", message);
        }
    }
}
