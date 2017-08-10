using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Services;
using Ubora.Web._Features.Home;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;
// ReSharper disable ArrangeAccessorOwnerBody

namespace Ubora.Web._Features
{
    public abstract class UboraController : Controller
    {
        protected virtual UserInfo UserInfo => User.GetInfo();

        protected Guid UserId => User.GetId();

        private UserProfile _userProfile;
        protected UserProfile UserProfile
        {
            get => _userProfile ?? (_userProfile = QueryProcessor.FindById<UserProfile>(UserId));
        }

        private IQueryProcessor _queryProcessor;
        protected IQueryProcessor QueryProcessor
        {
            get => _queryProcessor ?? (_queryProcessor = ServiceLocator.GetService<IQueryProcessor>());
        }

        private IAuthorizationService _authorizationService;
        protected IAuthorizationService AuthorizationService
        {
            get => _authorizationService ?? (_authorizationService = ServiceLocator.GetService<IAuthorizationService>());
        }

        private IMapper _autoMapper;
        protected IMapper AutoMapper
        {
            get => _autoMapper ?? (_autoMapper = ServiceLocator.GetService<IMapper>());
        }

        private ICommandProcessor _commandProcessor;
        private ICommandProcessor CommandProcessor
        {
            get => _commandProcessor ?? (_commandProcessor = ServiceLocator.GetService<ICommandProcessor>());
        }

        private IServiceProvider ServiceLocator => HttpContext.RequestServices;

        private NoticeQueue _notices;
        public NoticeQueue Notices
        {
            get => _notices ?? (_notices = new NoticeQueue(TempData));
        }

        protected void ShowNotice(Notice notice)
        {
            Notices.Enqueue(notice);
        }

        protected void ExecuteUserCommand<T>(T command) where T : IUserCommand
        {
            command.Actor = UserInfo;
            var result = CommandProcessor.Execute(command);
            if (result.IsFailure)
            {
                ModelState.AddCommandErrors(result);
            }
        }

        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
