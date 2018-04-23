using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Notifications;
using Ubora.Domain.Notifications.Commands;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Infrastructure;
using Ubora.Web._Features.Notifications._Base;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Notifications
{
    [Authorize]
    public class NotificationsController : UboraController
    {
        private readonly NotificationViewModelFactoryMediator _notificationViewModelFactoryMediator;

        public NotificationsController(NotificationViewModelFactoryMediator notificationViewModelFactoryMediator)
        {
            _notificationViewModelFactoryMediator = notificationViewModelFactoryMediator;
        }

        [RestoreModelStateFromTempData]
        public IActionResult Index()
        {
            var notifications = QueryProcessor.Find<INotification>(new IsForUser(UserId))
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            MarkNotificationsAsViewed();

            var viewModels = notifications.Select(_notificationViewModelFactoryMediator.Create);

            return View(viewModels);
        }

        private void MarkNotificationsAsViewed()
        {
            ExecuteUserCommand(new MarkNotificationsAsViewedCommand(), Notice.None("Background operation."));
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AcceptMentorInvitation(Guid invitationId)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            ExecuteUserCommand(new AcceptProjectMentorInvitationCommand
            {
                InvitationId = invitationId
            }, Notice.Success(SuccessTexts.ProjectMentorInvitationAccepted));

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction("Index", "Notifications");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult DeclineMentorInvitation(Guid invitationId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new DeclineProjectMentorInvitationCommand
            {
                InvitationId = invitationId
            }, Notice.Success(SuccessTexts.ProjectMentorInvitationDeclined));

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction("Index", "Notifications");
        }
    }
}