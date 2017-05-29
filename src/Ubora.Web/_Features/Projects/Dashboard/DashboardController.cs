using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class DashboardController : ProjectController
    {
        public DashboardController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        [Route(nameof(Dashboard))]
        public IActionResult Dashboard()
        {
            var model = new ProjectDashboardViewModel
            {
                Id = ProjectId,
                DeviceClassification = Project.DeviceClassification,
                Description = Project.Description
            };

            return View(model);
        }

        public IActionResult EditProjectDescription()
        {
            var editProjectDescription = new EditProjectDescriptionViewModel
            {
                ProjectDescription = Project.Description
            };

            return View(editProjectDescription);
        }

        [HttpPost]
        public IActionResult EditProjectDescription(EditProjectDescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                ExecuteUserProjectCommand(new UpdateProjectDescriptionCommand { ProjectId = ProjectId, Description = model.ProjectDescription });
                return RedirectToAction(nameof(Dashboard));
            }

            return View(model);
        }
    }
    public class EditProjectDescriptionViewModel
    {
        public string ProjectDescription { get; set; }
    }
}