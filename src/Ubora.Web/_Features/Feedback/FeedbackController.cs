using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Feedback
{
    public class FeedbackController : UboraController
    {
        [HttpPost]
        public IActionResult Send([FromBody]SendFeedbackCommand command)
        {
            if (!string.IsNullOrWhiteSpace(command.Feedback))
            {
                ExecuteUserCommand(command);
            }

            return Ok();
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public IActionResult All()
        {
            var all = QueryProcessor.Find<Feedback>(new MatchAll<Feedback>());
            return View(all);
        }
    }
}
