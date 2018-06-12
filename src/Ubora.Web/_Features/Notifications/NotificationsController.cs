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
using Ubora.Domain.Projects;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects._SortSpecifications;
using Ubora.Domain.Notifications.SortSpecifications;
using Ubora.Web._Features._Shared.Paging;

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
        public IActionResult Index(int page = 1)
        {
            var notifications = QueryProcessor.Find<INotification>(new IsForUser(UserId), new SortByCreatedAtDescendingSpecification(), 10, page);

            MarkNotificationsAsViewed();

            var model = new INotificationListViewModel
            {
                Pager = Pager.From(notifications),
                Notifications = notifications.Select(_notificationViewModelFactoryMediator.Create)
            };

            return View(model);
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