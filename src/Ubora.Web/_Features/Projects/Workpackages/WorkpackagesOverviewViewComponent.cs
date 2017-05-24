using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageOnes;
using Ubora.Web._Features.Projects.Workpackages.One;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackagesOverviewViewComponent : ProjectViewComponent
    {
        private readonly WorkpackageOneOverviewViewModel.Factory _workpackageOneFactoy;

        public WorkpackageOne WorkpackageOne => QueryProcessor.FindById<WorkpackageOne>(ProjectId);

        public WorkpackagesOverviewViewComponent(ICommandQueryProcessor processor, WorkpackageOneOverviewViewModel.Factory workpackageOneFactoy) 
            : base(processor)
        {
            _workpackageOneFactoy = workpackageOneFactoy;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new WorkpackagesOverviewViewModel();

            model.WorkpackageOne = _workpackageOneFactoy.Create();

            MarkSelectedItem(model);

            return View("~/_Features/Projects/Workpackages/OverviewPartial.cshtml", model);
        }

        private void MarkSelectedItem(WorkpackagesOverviewViewModel model)
        {
            var currentActionName = (string)RouteData.Values["action"];
            var currentControllerName = (string)RouteData.Values["controller"];

            foreach (var task in model.WorkpackageOne.Steps)
            {
                var isCurrentlyTaskAction = string.Equals(task.ActionName, currentActionName, StringComparison.OrdinalIgnoreCase);
                var isCurrentlyTaskController = string.Equals(task.ControllerName, currentControllerName, StringComparison.OrdinalIgnoreCase);

                if (isCurrentlyTaskAction
                    && isCurrentlyTaskController)
                {
                    task.IsSelected = true;
                    break;
                }
            }
        }
    }
}