using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ubora.Web.Data;
using Ubora.Web._Features._Shared.Templates;

namespace Ubora.Web.Services
{
    public interface IAuthMessageSender
    {
        Task SendEmailConfirmationMessage(ApplicationUser user);
        Task SendForgotPasswordMessage(ApplicationUser user);
    }

    public class AuthMessageSender : IAuthMessageSender
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IViewRender _view;

        public AuthMessageSender(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IViewRender view)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _view = view;
        }

        public async Task SendEmailConfirmationMessage(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var forgotPasswordMessageTemplateViewModel = new ForgotPasswordMessageTemplateViewModel
            {
                UserId = user.Id,
                Code = code
            };

            var message = _view.Render("~/_Features/_Shared/Templates/", "EmailConfirmationMessageTemplate.cshtml", forgotPasswordMessageTemplateViewModel);

            await _emailSender.SendEmailAsync(user.Email, "UBORA: e-mail confirmation", message);
        }

        public async Task SendForgotPasswordMessage(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var forgotPasswordMessageTemplateViewModel = new ForgotPasswordMessageTemplateViewModel
            {
                UserId = user.Id,
                Code = code
            };

            var message = _view.Render("~/_Features/_Shared/Templates/", "ForgotPasswordMessageTemplate.cshtml", forgotPasswordMessageTemplateViewModel);

            await _emailSender.SendEmailAsync(user.Email, "UBORA: Password reset", message);
        }
    }
}
