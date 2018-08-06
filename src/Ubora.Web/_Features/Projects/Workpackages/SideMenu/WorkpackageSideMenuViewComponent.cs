using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class WorkpackageSideMenuViewComponent : ProjectViewComponent
    {
        private readonly SideMenuViewModel.Factory _sideMenuFactory;

        public WorkpackageSideMenuViewComponent(IQueryProcessor processor, SideMenuViewModel.Factory sideMenuFactory) : base(processor)
        {
            _sideMenuFactory = sideMenuFactory;
        }

#pragma warning disable 1998
        public async Task<IViewComponentResult> InvokeAsync()
#pragma warning restore 1998
        {
            var viewModel = _sideMenuFactory.Create(
                projectId: ProjectId,
                selectedId: ViewData[nameof(WorkpackageMenuOption)] as string,
                user: UserClaimsPrincipal);

            return View("~/_Features/Projects/Workpackages/SideMenu/_WorkpackageSideMenuPartial.cshtml", viewModel);
        }
    }
}