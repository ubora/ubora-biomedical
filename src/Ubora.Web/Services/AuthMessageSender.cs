using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web.Data;

namespace Ubora.Web.Services
{
    public interface IAuthMessageSender
    {
        Task SendEmailConfirmationMessage(ApplicationUser user);
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

        public async Task SendEmailConfirmationMessage(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = _urlHelper.Action("ConfirmEmail", "Account", new { userId = user.Id, code },
                protocol: _urlHelper.ActionContext.HttpContext.Request.Scheme);

            var message = "<h1 style='color:#4777BB; font-family: sans-serif; text-align:center;'>E-mail confirmation</h1><p>Please confirm your e-mail by clicking here on the following link or copy-paste it in your browser: <br /><a href=\"" + callbackUrl + "\">" + callbackUrl + "</a>.</p>";

            await _emailSender.SendEmailAsync(user.Email, "UBORA: e-mail confirmation", message);
        }

        public async Task SendForgotPasswordMessageAsync(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = _urlHelper.Action("ResetPassword", "Account", new { userId = user.Id, code }, 
                protocol: _urlHelper.ActionContext.HttpContext.Request.Scheme);

            var message = "<h1 style='color:#4777BB; font-family: sans-serif; text-align:center;'>Password reset</h1><p style='font-family:sans-serif;'>You can reset your password by clicking on the following link or copy-paste it in your browser: <br /><a href=\"" + callbackUrl + "\">" + callbackUrl + "</a>.</p>";

            await _emailSender.SendEmailAsync(user.Email, "UBORA: Password reset", message);
        }
    }
}
