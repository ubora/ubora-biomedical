using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class DashboardController : ProjectController
    {
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public DashboardController(ICommandQueryProcessor processor, IMapper mapper, IAuthorizationService authorizationService) : base(processor)
        {
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Dashboard()
        {
            var model = _mapper.Map<ProjectDashboardViewModel>(Project);
            model.IsProjectMember = await _authorizationService.AuthorizeAsync(User, null, new IsProjectMemberRequirement());

            return View(nameof(Dashboard), model);
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