using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Feedback
{
    public class FeedbackController : UboraController
    {
        public FeedbackController(ICommandQueryProcessor processor) : base(processor)
        {
        }

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
            var all = Find<Feedback>();
            return View(all);
        }
    }
}
