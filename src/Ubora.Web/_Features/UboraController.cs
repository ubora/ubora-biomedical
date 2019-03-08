using System;
using System.Threading.Tasks;
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
using Microsoft.AspNetCore.NodeServices;
using Ubora.Domain;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;

// ReSharper disable ArrangeAccessorOwnerBody

namespace Ubora.Web._Features
{
    public abstract class UboraController : Controller
    {
        protected virtual UserInfo UserInfo => User.GetInfo();

        /// <remarks> Throws when unauthenticated. </remarks>>
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

        public string ExecutingActionName => (string)RouteData.Values["action"];

        protected void ExecuteUserCommand<T>(T command, Notice successNotice) where T : IUserCommand
        {
            command.Actor = UserInfo;
            var commandResult = CommandProcessor.Execute(command);
            if (commandResult.IsFailure)
            {
                ModelState.AddCommandErrors(commandResult);
                return;
            }

            if (successNotice.Type != NoticeType.None)
            {
                Notices.Enqueue(successNotice);
            }
        }

        protected async Task<string> ConvertQuillDeltaToHtml(QuillDelta quillDelta)
        {
            var nodeServices = ServiceLocator.GetService<INodeServices>();

            return await nodeServices.InvokeAsync<string>("./Scripts/backend/ConvertQuillDeltaToHtml.js", quillDelta?.Value);
        }

        protected async Task<string> SanitizeQuillDeltaForEditing(QuillDelta quillDelta)
        {
            var nodeServices = ServiceLocator.GetService<INodeServices>();

            return await nodeServices.InvokeAsync<string>("./Scripts/backend/SanitizeQuillDelta.js", quillDelta?.Value ?? new QuillDelta().Value);
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
