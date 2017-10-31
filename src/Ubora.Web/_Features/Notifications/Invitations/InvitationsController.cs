using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Infrastructure;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Notifications.Invitations
{
    [Authorize]
    public class InvitationsController : UboraController
    {
        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Accept(Guid invitationId)
        {
            if (!User.IsEmailConfirmed())
            {
                ModelState.AddModelError("", "You must confirm your email to join the project.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new AcceptInvitationToProjectCommand
            {
                InvitationId = invitationId
            });

            return RedirectToAction("Index", "Notifications");
        }

        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Decline(Guid invitationId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new DeclineInvitationToProjectCommand
            {
                InvitationId = invitationId
            });

            return RedirectToAction("Index", "Notifications");
        }
    }
}