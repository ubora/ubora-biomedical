using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Features.Projects.History._Base;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.History;
using Ubora.Domain.Projects._Specifications;

namespace Ubora.Web._Features.Projects.History
{
    [ProjectRoute("[controller]")]
    public class HistoryController : ProjectController
    {
        private readonly IEventViewModelFactoryMediator _eventViewModelFactoryMediator;

        public HistoryController(IEventViewModelFactoryMediator eventViewModelFactoryMediator)
        {
            _eventViewModelFactoryMediator = eventViewModelFactoryMediator;
        }

        public IActionResult History()
        {
            var logs = QueryProcessor.Find(new IsFromProjectSpec<EventLogEntry>() {ProjectId = ProjectId})
                .OrderByDescending(l => l.Timestamp);
            var viewModels = logs.Select(x => _eventViewModelFactoryMediator.Create((UboraEvent)x.Event, x.Timestamp)).ToArray();

            return View(viewModels);
        }
    }
}
