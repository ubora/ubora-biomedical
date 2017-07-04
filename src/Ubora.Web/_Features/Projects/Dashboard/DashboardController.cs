using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class DashboardController : ProjectController
    {
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IStorageProvider _storageProvider;

        public DashboardController(
            ICommandQueryProcessor processor,
            IMapper mapper,
            IAuthorizationService authorizationService,
            IStorageProvider storageProvider) : base(processor)
        {
            _mapper = mapper;
            _authorizationService = authorizationService;
            _storageProvider = storageProvider;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Dashboard()
        {
            var model = _mapper.Map<ProjectDashboardViewModel>(Project);
            model.IsProjectMember = await _authorizationService.AuthorizeAsync(User, null, new IsProjectMemberRequirement());
            model.ImagePath = _storageProvider.GetDefaultOrBlobUrl(Project);

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

        public IActionResult EditProjectImage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditProjectImage(EditProjectImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditProjectImage();
            }

            ExecuteUserProjectCommand(new UpdateProjectImageCommand
            {
                ProjectId = ProjectId,
                Actor = UserInfo,
                ImageName = model.FileName,
                Stream = model.ProjectImage.OpenReadStream()
            });

            if (!ModelState.IsValid)
            {
                return EditProjectImage();
            }

            return RedirectToAction(nameof(Dashboard));
        }
    }
}