using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Notifications;

namespace Ubora.Web._Features.Notifications.Invitations
{
    [Authorize]
    public class InvitationsController : UboraController
    {
        public InvitationsController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        [HttpPost]
        public IActionResult Accept(InvitationPartialViewModel invitationPartialViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ExecuteUserCommand(new AcceptInvitationToProjectCommand { InvitationId = invitationPartialViewModel.InviteId });

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Notifications");
        }

        [HttpPost]
        public IActionResult Decline(InvitationPartialViewModel invitationPartialViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ExecuteUserCommand(new DeclineInvitationToProjectCommand { InvitationId = invitationPartialViewModel.InviteId });

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Notifications");
        }
    }
}