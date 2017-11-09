using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using TwentyTwenty.Storage;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Authorization;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web._Features.Home;

namespace Ubora.Web._Features.Projects.Dashboard
{
    [ProjectRoute("[controller]")]
    public class DashboardController : ProjectController
    {
        private readonly IStorageProvider _storageProvider;
        private readonly ImageStorageProvider _imageStorage;

        public DashboardController(
            IStorageProvider storageProvider,
            ImageStorageProvider imageStorage)
        {
            _storageProvider = storageProvider;
            _imageStorage = imageStorage;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Dashboard()
        {
            var model = AutoMapper.Map<ProjectDashboardViewModel>(Project);
            model.IsProjectMember = (await AuthorizationService.AuthorizeAsync(User, null, new IsProjectMemberRequirement())).Succeeded;

            model.HasImage = Project.HasImage;
            if (Project.HasImage)
            {
                model.ImagePath = _imageStorage.GetUrl(Project.ProjectImageBlobLocation, ImageSize.Thumbnail400x300);
            }

            return View(nameof(Dashboard), model);
        }

        [Route(nameof(EditProjectDescription))]
        [Authorize(Policies.CanEditProjectDescription)]
        public IActionResult EditProjectDescription()
        {
            var editProjectDescription = new EditProjectDescriptionViewModel
            {
                ProjectDescription = Project.Description
            };

            return View(editProjectDescription);
        }

        [HttpPost]
        [Route(nameof(EditProjectDescription))]
        [Authorize(Policies.CanEditProjectDescription)]
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

        [Route(nameof(EditProjectImage))]
        [Authorize(Policies.CanChangeProjectImage)]
        public IActionResult EditProjectImage()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(EditProjectImage))]
        [Authorize(Policies.CanChangeProjectImage)]
        public async Task<IActionResult> EditProjectImage(EditProjectImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditProjectImage();
            }

            var imageStream = model.Image.OpenReadStream();
            var blobLocation = BlobLocations.GetProjectImageBlobLocation(ProjectId);
            await _imageStorage.SaveImageAsync(imageStream, blobLocation, SizeOptions.AllDefaultSizes);

            ExecuteUserProjectCommand(new UpdateProjectImageCommand
            {
                ProjectId = ProjectId,
                BlobLocation = blobLocation,
                Actor = UserInfo
            });

            if (!ModelState.IsValid)
            {
                return EditProjectImage();
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [Route(nameof(RemoveProjectImage))]
        public IActionResult RemoveProjectImage()
        {
            var model = new RemoveProjectImageViewModel
            {
                Title = Project.Title
            };

            return View(model);
        }

        [HttpPost]

        [Route(nameof(RemoveProjectImage))]
        public async Task<IActionResult> RemoveProjectImage(RemoveProjectImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RemoveProjectImage();
            }

            await _imageStorage.DeleteImagesAsync(BlobLocations.GetProjectImageBlobLocation(ProjectId));

            ExecuteUserProjectCommand(new DeleteProjectImageCommand
            {
                ProjectId = ProjectId,
                Actor = UserInfo
            });

            if (!ModelState.IsValid)
            {
                return RemoveProjectImage();
            }

            return RedirectToAction(nameof(Dashboard));
        }
    }
}