using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web._Components;

namespace Ubora.Web._Features.Home
{
    public class LandingPageRandomProjectsRow : ViewComponent
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ProjectCardViewModel.Factory _viewModelFactory;

        public LandingPageRandomProjectsRow(IQueryProcessor queryProcessor, ProjectCardViewModel.Factory viewModelFactory)
        {
            _queryProcessor = queryProcessor;
            _viewModelFactory = viewModelFactory;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = _queryProcessor
                .ExecuteQuery(new LandingPageRandomProjectsQuery())
                .Take(5)
                .Select(project => _viewModelFactory.Create(project, showCardShadow: false));

            var result = (IViewComponentResult) View("~/_Features/Home/LandingPageRandomProjectsRow.cshtml", viewModels);
            return Task.FromResult(result);
        }
    }
}