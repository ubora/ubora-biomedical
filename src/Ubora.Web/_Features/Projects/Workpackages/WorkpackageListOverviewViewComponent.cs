using System;
using System.Collections.Generic;
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
            // TODO(Kaspar Kallas): Cleaner solution!
            var wp1 = QueryProcessor.FindById<Domain.Projects.Workpackages.WorkpackageOne>(ProjectId);
            var workpackageViewModels = new List<WorkpackageOneOverviewViewModel>
            {
                CreateViewModel(wp1)
            };

            // TODO(Kaspar Kallas): Cleaner solution!
            var wp2 = QueryProcessor.FindById<WorkpackageTwo>(ProjectId);
            if (wp2 != null)
            {
                workpackageViewModels.Add(CreateViewModel(wp2));
            }

            var model = new WorkpackageListOverviewViewModel
            {
                Workpackages = workpackageViewModels
            };

            var selectedStepId = GetSelectedStepId();
            if (selectedStepId != null)
            {
                model.MarkSelectedStep(selectedStepId.Value);
            }

            return View("~/_Features/Projects/Workpackages/_WorkpackageListOverviewPartial.cshtml", model);
        }

        private WorkpackageOneOverviewViewModel CreateViewModel<T>(Workpackage<T> workpackage) where T : Workpackage<T>
        {
            var model = new WorkpackageOneOverviewViewModel
            {
                Title = workpackage.Title,
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