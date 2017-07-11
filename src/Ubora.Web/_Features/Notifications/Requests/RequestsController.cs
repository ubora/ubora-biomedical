using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Notifications.Join;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Notifications.Requests
{
    [Authorize]
    public class RequestsController : UboraController
    {
        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Accept(RequestPartialViewModel requestPartialViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new AcceptRequestToJoinProjectCommand
            {
                RequestId = requestPartialViewModel.RequestId
            });

            return RedirectToAction("Index", "Notifications");
        }

        [HttpPost]
        [SaveTempDataModelState]
        public IActionResult Decline(RequestPartialViewModel requestPartialViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Notifications");
            }

            ExecuteUserCommand(new DeclineRequestToJoinProjectCommand
            {
                RequestId = requestPartialViewModel.RequestId
            });

            return RedirectToAction("Index", "Notifications");
        }
    }
}