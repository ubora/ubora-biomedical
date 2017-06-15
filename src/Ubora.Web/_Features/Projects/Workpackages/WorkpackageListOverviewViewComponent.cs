using System;
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

        public Task<IViewComponentResult> InvokeAsync()
        {
            var model = new WorkpackageListOverviewViewModel
            {
                Workpackages = new[]
                {
                    CreateViewModel<Domain.Projects.Workpackages.WorkpackageOne>(),
                    CreateViewModel<WorkpackageTwo>(),
                    CreateViewModel<WorkpackageThree>(),
                    CreateViewModel<WorkpackageFour>(),
                    CreateViewModel<WorkpackageFive>()
                }
            };

            var selectedStepId = GetSelectedStepId();
            if (selectedStepId != null)
            {
                model.MarkSelectedStep(selectedStepId.Value);
            }

            return Task.FromResult<IViewComponentResult>(
                View("~/_Features/Projects/Workpackages/_WorkpackageListOverviewPartial.cshtml", model));
        }

        private WorkpackageOneOverviewViewModel CreateViewModel<TWorkpackage>()
            where TWorkpackage : Workpackage<TWorkpackage>
        {
            var workpackage = QueryProcessor.FindById<TWorkpackage>(ProjectId);

            var model = new WorkpackageOneOverviewViewModel
            {
                Title = workpackage.Title,
                IsVisible = workpackage.IsVisible,
                Steps = workpackage.Steps.Select(task => new WorkpackageOneOverviewViewModel.Step
                {
                    Id = task.Id,
                    Title = task.Title
                }).ToList()
            };

            return model;
        }

        private Guid? GetSelectedStepId()
        {
            var projectIdFromRoute = RouteData.Values["id"] as string;
            Guid.TryParse(projectIdFromRoute, out Guid stepId);

            if (stepId == default(Guid))
            {
                return null;
            }

            return stepId;
        }
    }
}