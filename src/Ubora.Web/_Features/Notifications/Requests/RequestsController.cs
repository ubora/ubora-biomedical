using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Notifications.Join;

namespace Ubora.Web._Features.Notifications.Requests
{
    [Authorize]
    public class RequestsController : UboraController
    {
        public RequestsController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        [HttpPost]
        public IActionResult Accept(RequestPartialViewModel requestPartialViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ExecuteUserCommand(new AcceptRequestToJoinProjectCommand {  RequestId = requestPartialViewModel.RequestId });

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Notifications");
        }

        [HttpPost]
        public IActionResult Decline(RequestPartialViewModel requestPartialViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ExecuteUserCommand(new DeclineRequestToJoinProjectCommand { RequestId = requestPartialViewModel.RequestId });

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Notifications");
        }
    }
}