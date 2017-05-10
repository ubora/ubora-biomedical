using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web.Services;

namespace Ubora.Web.Features.Projects
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

            return View("~/Features/Projects/ProjectListPartial.cshtml", model);
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
            var model = _modelFactory.Create("My projects", User.GetId());

            return View("~/Features/Projects/ProjectListPartial.cshtml", model);
        }
    }
}
