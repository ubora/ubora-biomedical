using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Web._Features.Projects.History
{
    [ProjectRoute("[controller]/")]
    public class HistoryController : ProjectController
    {
        private readonly IEventStreamQuery _eventStreamQuery;

        public HistoryController(ICommandQueryProcessor processor, IEventStreamQuery eventStreamQuery) : base(processor)
        {
            _eventStreamQuery = eventStreamQuery;
        }

        public IActionResult History()
        {
            var projectEventStream = _eventStreamQuery.Find(ProjectId);

            var model = new ProjectHistoryViewModel
            {
                Events = projectEventStream.Select(x => x.ToString())
            };

            return View(model);
        }
    }
}
