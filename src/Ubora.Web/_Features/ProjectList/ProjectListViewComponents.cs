using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web.Services;

namespace Ubora.Web._Features.ProjectList
{
    public class PublicProjectListViewComponent : ViewComponent
    {
        private readonly ProjectListViewModel.Factory _modelFactory;

        public PublicProjectListViewComponent(ProjectListViewModel.Factory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _modelFactory.Create(header: "Public projects");

            return View("~/_Features/ProjectList/ProjectListPartial.cshtml", model);
        }
    }

    public class MyProjectListViewComponent : ViewComponent
    {
        private readonly ProjectListViewModel.Factory _modelFactory;

        public MyProjectListViewComponent(ProjectListViewModel.Factory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _modelFactory.Create(header: "My projects", userId: User.GetId());

            return View("~/_Features/ProjectList/ProjectListPartial.cshtml", model);
        }
    }
}
