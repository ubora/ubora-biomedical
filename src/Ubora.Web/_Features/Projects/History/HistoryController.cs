using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Web._Features.Projects.History
{
    public class HistoryController : ProjectController
    {
        private readonly IEventStreamQuery _eventStreamQuery;

        public HistoryController(IEventStreamQuery eventStreamQuery)
        {
            _eventStreamQuery = eventStreamQuery;
        }

        public IActionResult History(Guid id)
        {
            var projectEventStream = _eventStreamQuery.Find(id);

            var model = new ProjectHistoryViewModel
            {
                Events = projectEventStream.Select(x => x.ToString())
            };

            return View(model);
        }
    }
}
