using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.ProjectList
{
    public class SearchingProjectListViewComponent : ViewComponent
    {
        private readonly ProjectListViewModel.Factory _modelFactory;

        public SearchingProjectListViewComponent(ProjectListViewModel.Factory modelFactory)
        {
            _modelFactory = modelFactory;
        }

#pragma warning disable 1998
        public async Task<IViewComponentResult> InvokeAsync(string title)
#pragma warning restore 1998
        {
            var model = _modelFactory.CreateForSearch(title);

            return View("~/_Features/ProjectList/ProjectListPartial.cshtml", model);
        }
    }
}
