using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackageListOverviewViewModel : ProjectViewComponent
    {
        public WorkpackageOne WorkpackageOne => QueryProcessor.FindById<WorkpackageOne>(ProjectId);

        public WorkpackageListOverviewViewModel(ICommandQueryProcessor processor) 
            : base(processor)
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new WorkpackageOneOverviewViewModel
            {
                Title = WorkpackageOne.Title,
                Steps = WorkpackageOne.Steps.Select(step => new WorkpackageOneOverviewViewModel.Step
                {
                    Id = step.Id,
                    Title = step.Title
                }).ToList()
            };

            MarkSelectedItem(model);

            return View("~/_Features/Projects/Workpackages/_Version2OverviewPartial.cshtml", model);
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