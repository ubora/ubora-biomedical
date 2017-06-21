using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class DashboardController : ProjectController
    {
        private readonly IMapper _mapper;

        public DashboardController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        public IActionResult Dashboard()
        {
            var model = _mapper.Map<ProjectDashboardViewModel>(Project);

            return View(nameof(Dashboard), model);
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost]
        public IActionResult AssignMeAsMentor()
        {
            ExecuteUserProjectCommand(new AssignProjectMentorCommand
            {
                UserId = this.UserId
            });

            if (!ModelState.IsValid)
            {
                return Dashboard();
            }

            return RedirectToAction(nameof(Dashboard));
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
            if (!ModelState.IsValid)
            {
                return EditProjectDescription();
            }

            ExecuteUserProjectCommand(new UpdateProjectDescriptionCommand
            {
                ProjectId = ProjectId,
                Description = model.ProjectDescription
            });

            if (!ModelState.IsValid)
            {
                return EditProjectDescription();
            }

            return RedirectToAction(nameof(Dashboard));
        }
    }
}