using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ubora.Web.Data;
using Ubora.Web.Services;
using Ubora.Web._Features.Users.Account;
using Ubora.Web._Features._Shared.Templates;

namespace Ubora.Web.Infrastructure
{
    public class AuthMessageSender : IPasswordRecoveryMessageSender, IEmailConfirmationMessageSender
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ViewRender _view;

        public AuthMessageSender(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ViewRender view)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _view = view;
        }

        public async Task SendEmailConfirmationMessage(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var uboraLogoContentId = "uboraLogoIcon";
            var emailLogoContentId = "emailLogoIcon";
            var facebookLogoContentId = "facebookLogoIcon";
            var twitterLogoContentId = "twitterLogoIcon";
            var emailIcons = new[]
            {
                new EmailIcon("./wwwroot/images/icon.png", uboraLogoContentId),
                new EmailIcon("./wwwroot/images/icons/email_icon.png", emailLogoContentId),
                new EmailIcon("./wwwroot/images/icons/facebook_icon.png", facebookLogoContentId),
                new EmailIcon("./wwwroot/images/icons/twitter_icon.png", twitterLogoContentId)
            };

            var callBackUrlTemplateViewModel = new CallBackUrlTemplateViewModel
            {
                UserId = user.Id,
                Code = code,
                UboraLogoContentId = uboraLogoContentId,
                EmailLogoContentId = emailLogoContentId,
                FacebookLogoContentId = facebookLogoContentId,
                TwitterLogoContentId = twitterLogoContentId
            };

            var message = _view.Render("/_Features/_Shared/Templates/", "EmailConfirmationMessageTemplate.cshtml", callBackUrlTemplateViewModel);

            var result = PreMailer.Net.PreMailer.MoveCssInline(message, removeStyleElements: true);

            await _emailSender.SendEmailAsync(user.Email, "UBORA: e-mail confirmation", result.Html, emailIcons);
        }

        public async Task SendForgotPasswordMessage(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var uboraLogoContentId = "uboraLogoIcon";
            var emailLogoContentId = "emailLogoIcon";
            var facebookLogoContentId = "facebookLogoIcon";
            var twitterLogoContentId = "twitterLogoIcon";
            var emailIcons = new[]
            {
                new EmailIcon("./wwwroot/images/icon.png", uboraLogoContentId),
                new EmailIcon("./wwwroot/images/icons/email_icon.png", emailLogoContentId),
                new EmailIcon("./wwwroot/images/icons/facebook_icon.png", facebookLogoContentId),
                new EmailIcon("./wwwroot/images/icons/twitter_icon.png", twitterLogoContentId)
            };

            var callBackUrlTemplateViewModel = new CallBackUrlTemplateViewModel
            {
                UserId = user.Id,
                Code = code,
                UboraLogoContentId = uboraLogoContentId,
                EmailLogoContentId = emailLogoContentId,
                FacebookLogoContentId = facebookLogoContentId,
                TwitterLogoContentId = twitterLogoContentId
            };

            var message = _view.Render("/_Features/_Shared/Templates/", "ForgotPasswordMessageTemplate.cshtml", callBackUrlTemplateViewModel);

            var result = PreMailer.Net.PreMailer.MoveCssInline(message, removeStyleElements: true);

            await _emailSender.SendEmailAsync(user.Email, "UBORA: Password reset", result.Html, emailIcons);
        }
    }


    public class EmailIcon
    {
        public EmailIcon(string path, string contentId)
        {
            Path = path;
            ContentId = contentId;
        }

        public string Path { get; set; }
        public string ContentId { get; set; }
    }
}
