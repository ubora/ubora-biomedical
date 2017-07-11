using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Notifications.Invitation;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Notifications.Invitations
{
    [Authorize]
    public class InvitationsController : UboraController
    {
        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Accept(InvitationPartialViewModel invitationPartialViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new AcceptInvitationToProjectCommand
            {
                InvitationId = invitationPartialViewModel.InviteId
            });

            return RedirectToAction("Index", "Notifications");
        }

        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Decline(InvitationPartialViewModel invitationPartialViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new DeclineInvitationToProjectCommand
            {
                InvitationId = invitationPartialViewModel.InviteId
            });

            return RedirectToAction("Index", "Notifications");
        }
    }
}