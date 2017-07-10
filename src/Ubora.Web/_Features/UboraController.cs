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
// ReSharper disable ArrangeAccessorOwnerBody

namespace Ubora.Web._Features
{
    public abstract class UboraController : Controller
    {
        protected virtual UserInfo UserInfo => User.GetInfo();

        protected Guid UserId => User.GetId();

        private UserProfile _userProfile;
        protected UserProfile UserProfile => _userProfile ?? (_userProfile = QueryProcessor.FindById<UserProfile>(UserId));

        private IQueryProcessor _queryProcessor;
        public IQueryProcessor QueryProcessor
        {
            get => _queryProcessor ?? (_queryProcessor = HttpContext.RequestServices.GetService<IQueryProcessor>());
            set { _queryProcessor = value; }
        }

        private ICommandProcessor _commandProcessor;
        public ICommandProcessor CommandProcessor
        {
            get => _commandProcessor ?? (_commandProcessor = HttpContext.RequestServices.GetService<ICommandProcessor>());
            set { _commandProcessor = value; }
        }

        private IMapper _autoMapper;
        public IMapper AutoMapper
        {
            get => _autoMapper ?? (_autoMapper = HttpContext.RequestServices.GetService<IMapper>());
            set { _autoMapper = value; }
        }

        private IAuthorizationService _authorizationService;
        public IAuthorizationService AuthorizationService
        {
            get => _authorizationService ?? (_authorizationService = HttpContext.RequestServices.GetService<IAuthorizationService>());
            set { _authorizationService = value; }
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
