using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Infrastructure;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Notifications.Requests
{
    [Authorize]
    public class RequestsController : UboraController
    {
        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Accept([NotDefault]Guid requestId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new AcceptRequestToJoinProjectCommand
            {
                RequestId = requestId
            }, Notice.Success(SuccessTexts.RequestToJoinProjectAccepted));

            return RedirectToAction("Index", "Notifications");
        }

        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Decline([NotDefault]Guid requestId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new DeclineRequestToJoinProjectCommand
            {
                RequestId = requestId
            }, Notice.Success(SuccessTexts.RequestToJoinProjectDeclined));

            return RedirectToAction("Index", "Notifications");
        }
    }
}