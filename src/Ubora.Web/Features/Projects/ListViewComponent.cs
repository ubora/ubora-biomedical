using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Web.Features.Projects
{
    public class ListViewComponent : ViewComponent
    {
        private readonly IQueryProcessor _processor;

        public ListViewComponent(IQueryProcessor processor)
        {
            _processor = processor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var projects = _processor.Find<Project>();

            var viewModel = new ListViewModel
            {
                Projects = projects.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Title })
            };

            return View("~/Features/Projects/ListPartial.cshtml", viewModel);
        }
    }
}
