using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Notifications.Commands;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Infrastructure;
using Ubora.Web._Features.Notifications._Base;

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
            var notifications = QueryProcessor.Find(new HasPendingNotifications(UserId))
                .ToList();

            MarkNotificationsAsViewed();

            var viewModels = notifications.Select(_notificationViewModelFactoryMediator.Create);

            return View(viewModels);
        }

        public IActionResult History()
        {
            var notifications = QueryProcessor.Find(new HasArchivedNotifications(UserId))
                .ToList();

            var viewModels = notifications.Select(_notificationViewModelFactoryMediator.Create);

            return View(viewModels);
        }

        private void MarkNotificationsAsViewed()
        {
            ExecuteUserCommand(new MarkNotificationsAsViewedCommand());
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
            });

            if (!ModelState.IsValid)
            {
                return Index();
            }

            // TODO: Notice

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
            });

            if (!ModelState.IsValid)
            {
                return Index();
            }

            // TODO: Notice

            return RedirectToAction("Index", "Notifications");
        }
    }
}