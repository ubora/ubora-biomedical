using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Infrastructure;
using System.Threading.Tasks;
using Ubora.Web.Authorization;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Notifications.Invitations
{
    [Authorize]
    public class InvitationsController : UboraController
    {
        [HttpPost]
        [SaveTempDataModelState]
        public async Task<IActionResult> Accept(Guid invitationId)
        {
            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanJoinProject))
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
            }, Notice.Success(SuccessTexts.ProjectInvitationAccepted));

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
            }, Notice.Success(SuccessTexts.ProjectInvitationDeclined));

            return RedirectToAction("Index", "Notifications");
        }
    }
}