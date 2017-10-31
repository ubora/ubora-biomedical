using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure.PreMailers;
using Ubora.Web.Services;
using Ubora.Web._Features.Users.Account;
using Ubora.Web._Features._Shared.Emails;

namespace Ubora.Web.Infrastructure
{
    public class ApplicationUserEmailMessageSender : IPasswordRecoveryMessageSender, IEmailConfirmationMessageSender
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailSender _emailSender;
        private readonly ViewRender _viewRender;
        private readonly PreMailerFactory _preMailerFactory;

        public ApplicationUserEmailMessageSender(ApplicationUserManager userManager, EmailSender emailSender, ViewRender viewRender, PreMailerFactory preMailerFactory)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _viewRender = viewRender;
            _preMailerFactory = preMailerFactory;
        }

        public async Task SendEmailConfirmationMessage(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var viewModel = new EmailConfirmationEmailViewModel
            {
                UserId = user.Id,
                Code = code,
            };

            var view = _viewRender.Render("/_Features/_Shared/Emails/", "EmailConfirmationMessageTemplate.cshtml", viewModel);

            var messageFinalHtml = _preMailerFactory.Create(view)
                .MoveCssInline(removeStyleElements: true, ignoreElements: ".ignore-premailer")
                .Html;

            await _emailSender.SendEmailAsync(user.Email, "UBORA: e-mail confirmation", messageFinalHtml, handleLinkedResources: EmailLayoutViewModel.AddLayoutAttachments);
        }

        public async Task SendForgotPasswordMessage(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var viewModel = new ForgotPasswordEmailViewModel
            {
                UserId = user.Id,
                Code = code
            };

            var view = _viewRender.Render("/_Features/_Shared/Emails/", "ForgotPasswordMessageTemplate.cshtml", viewModel);

            var messageFinalHtml = _preMailerFactory.Create(view)
                .MoveCssInline(removeStyleElements: true, ignoreElements: ".ignore-premailer")
                .Html;

            await _emailSender.SendEmailAsync(user.Email, "UBORA: Password reset", messageFinalHtml, handleLinkedResources: EmailLayoutViewModel.AddLayoutAttachments);
        }
    }
}