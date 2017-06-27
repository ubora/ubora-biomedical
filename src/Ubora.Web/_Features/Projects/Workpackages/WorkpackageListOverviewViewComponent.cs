using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOne;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackageListOverviewViewComponent : ProjectViewComponent
    {
        public WorkpackageListOverviewViewComponent(IQueryProcessor processor)
            : base(processor)
        {
        }

#pragma warning disable 1998
        public async Task<IViewComponentResult> InvokeAsync()
#pragma warning restore 1998
        {
            var model = new WorkpackageListOverviewViewModel
            {
                WorkpackageOne = CreateViewModel<Domain.Projects.Workpackages.WorkpackageOne>(),
                WorkpackageTwo = CreateViewModel<WorkpackageTwo>(),
                WorkpackageThree = CreateViewModel<WorkpackageThree>()
            };

            var selectedStepId = GetSelectedStepId();
            if (selectedStepId != null)
            {
                model.MarkSelectedStep(selectedStepId);
            }

            return View("~/_Features/Projects/Workpackages/_WorkpackageListOverviewPartial.cshtml", model);
        }

        private WorkpackageOneOverviewViewModel CreateViewModel<T>() where T : Workpackage<T>
        {
            var workpackage = QueryProcessor.FindById<T>(ProjectId);
            if (workpackage == null)
            {
                // TODO
                return null;
            }

            var model = new WorkpackageOneOverviewViewModel
            {
                Title = workpackage.Title,
                Steps = workpackage.Steps.Select(task => new WorkpackageOneOverviewViewModel.Step
                {
                    Id = task.Id.ToString(),
                    Title = task.Title
                }).ToList()
            };

            return model;
        }

        private string GetSelectedStepId()
        {
            return RouteData.Values["stepId"] as string;
        }
    }
}