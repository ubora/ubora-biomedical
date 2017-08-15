using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Notifications.Invitations
{
    [Authorize]
    public class InvitationsController : UboraController
    {
        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Accept([NotDefault]Guid inviteId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new AcceptInvitationToProjectCommand
            {
                InvitationId = inviteId
            });

            return RedirectToAction("Index", "Notifications");
        }

        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Decline([NotDefault]Guid inviteId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new DeclineInvitationToProjectCommand
            {
                InvitationId = inviteId
            });

            return RedirectToAction("Index", "Notifications");
        }
    }
}