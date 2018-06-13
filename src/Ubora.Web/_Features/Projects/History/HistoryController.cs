using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Features.Projects.History._Base;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.History;
using Ubora.Domain.Projects.History.SortSpecifications;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web._Features._Shared.Paging;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Authorization;

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

        [DisableProjectControllerAuthorization]
        [Authorize(Policy = nameof(Policies.CanViewProjectHistory))]
        public IActionResult History(int page = 1)
        {
            var logs = 
                QueryProcessor.Find(
                    new IsFromProjectSpec<EventLogEntry> { ProjectId = ProjectId },
                    new SortByTimestampDescendingSpecification(), 10, page);
            
           var logViewModels = logs.Select(x => _eventViewModelFactoryMediator.Create((UboraEvent)x.Event, x.Timestamp)).ToArray();

            return View(new HistoryViewModel
            {
                Pager = Pager.From(logs),
                Logs = logViewModels
            });
        }
    }

    public class HistoryViewModel
    {
        public Pager Pager { get; set; }
        public IEventViewModel[] Logs { get; set; }
    }
}
