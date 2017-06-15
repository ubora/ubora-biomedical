using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOne;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackageListOverviewViewComponent : ProjectViewComponent
    {
        public WorkpackageListOverviewViewComponent(ICommandQueryProcessor processor)
            : base(processor)
        {
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var wp1 = CreateWorkpackageModel<Domain.Projects.Workpackages.WorkpackageOne>();
            var wp2 = CreateWorkpackageModel<WorkpackageTwo>();
            var wp3 = CreateWorkpackageModel<WorkpackageThree>();
            var wp4 = CreateWorkpackageModel<WorkpackageFour>();
            var wp5 = CreateWorkpackageModel<WorkpackageFive>();

            var model = new WorkpackageListOverviewViewModel
            {
                Workpackages = new []
                {
                    wp1,
                    wp2,
                    wp3,
                    wp4,
                    wp5
                }
            };

            return Task.FromResult<IViewComponentResult>(
                View("~/_Features/Projects/Workpackages/_WorkpackageListOverviewPartial.cshtml", model));
        }

        private WorkpackageOneOverviewViewModel CreateWorkpackageModel<TWorkpackage>() where TWorkpackage : Workpackage<TWorkpackage>
        {
            var wp = QueryProcessor.FindById<TWorkpackage>(ProjectId);

            var model = new WorkpackageOneOverviewViewModel
            {
                Title = wp.Title,
                IsVisible = wp.IsVisible,
                Steps = wp.Steps.Select(step => new WorkpackageOneOverviewViewModel.Step
                {
                    Id = step.Id,
                    Title = step.Title
                })
            };

            MarkSelectedItem(model);

            return model;
        }

        private void MarkSelectedItem(WorkpackageOneOverviewViewModel model)
        {
            var id = (string)RouteData.Values["id"];

            foreach (var task in model.Steps)
            {
                if (id == task.Id.ToString())
                {
                    task.IsSelected = true;
                    break;
                }
            }
        }
    }
}