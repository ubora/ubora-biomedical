using Microsoft.AspNetCore.Mvc;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Notifications;

namespace Ubora.Web._Features.Notifications
{
    public class InvitationsController : UboraController
    {
        public InvitationsController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult Accept(Guid invitationId)
        {
            ExecuteUserCommand(new AcceptMemberInvitationToProjectCommand { InvitationId = invitationId });

            return RedirectToAction("Index", "Notifications");
        }

        public IActionResult Decline(Guid invitationId)
        {
            ExecuteUserCommand(new DeclineMemberInvitationToProjectCommand { InvitationId = invitationId });

            return RedirectToAction("Index", "Notifications");
        }
    }
}