using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Features.Projects.History._Base;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Web._Features.Projects.History
{
    [ProjectRoute("[controller]")]
    public class HistoryController : ProjectController
    {
        private readonly EventViewModelFactoryMediator _eventViewModelFactoryMediator;
        private readonly IEventStore _eventStore;

        public HistoryController(EventViewModelFactoryMediator eventViewModelFactoryMediator, IEventStore eventStore)
        {
            _eventViewModelFactoryMediator = eventViewModelFactoryMediator;
            _eventStore = eventStore;
        }

        public IActionResult History()
        {
            var projectEvents = _eventStore.FetchStream(ProjectId)
                .OrderByDescending(x => x.Timestamp);

            var viewModels = projectEvents.Select(x => _eventViewModelFactoryMediator.Create((UboraEvent)x.Data, x.Timestamp));

            return View(viewModels);
        }
    }
}
