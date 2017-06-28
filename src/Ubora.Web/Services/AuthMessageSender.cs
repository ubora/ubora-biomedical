﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
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
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IEmailSender _emailSender;

        public AuthMessageSender(UserManager<ApplicationUser> userManager, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor, IEmailSender emailSender)
        {
            _userManager = userManager;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _emailSender = emailSender;
        }

        public async Task SendForgotPasswordMessageAsync(ApplicationUser user)
        {
            var subject = "Reset Password";
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext)
                .Action("ResetPassword", "Account", new { userId = user.Id, code },
                    _actionContextAccessor.ActionContext.HttpContext.Request.Scheme);
            var message = "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>";
            await _emailSender.SendEmailAsync(user.Email, subject, message);
        }
    }
}
