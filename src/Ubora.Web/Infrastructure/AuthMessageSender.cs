﻿using System.Threading.Tasks;
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

            var callBackUrlTemplateViewModel = new CallBackUrlTemplateViewModel
            {
                UserId = user.Id,
                Code = code
            };

            var message = _view.Render("~/_Features/_Shared/Templates/", "EmailConfirmationMessageTemplate.cshtml", callBackUrlTemplateViewModel);

            await _emailSender.SendEmailAsync(user.Email, "UBORA: e-mail confirmation", message);
        }

        public async Task SendForgotPasswordMessage(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callBackUrlTemplateViewModel = new CallBackUrlTemplateViewModel
            {
                UserId = user.Id,
                Code = code
            };

            var message = _view.Render("~/_Features/_Shared/Templates/", "ForgotPasswordMessageTemplate.cshtml", callBackUrlTemplateViewModel);

            await _emailSender.SendEmailAsync(user.Email, "UBORA: Password reset", message);
        }
    }
}