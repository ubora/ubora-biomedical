using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;

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
        public async Task<IActionResult> Dashboard([FromServices]ProjectDashboardViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(Project, UserId);

            return View(nameof(Dashboard), model);
        }

        [Route(nameof(EditProjectTitleAndDescription))]
        [Authorize(Policies.CanEditProjectTitleAndDescription)]
        public IActionResult EditProjectTitleAndDescription()
        {
            var editProjectDescription = new EditProjectTitleAndDescriptionViewModel
            {
                ProjectDescription = Project.Description,
                Title = Project.Title
            };

            return View(nameof(EditProjectTitleAndDescription),editProjectDescription);
        }

        [HttpPost]
        [Route(nameof(EditProjectTitleAndDescription))]
        [Authorize(Policies.CanEditProjectTitleAndDescription)]
        public IActionResult EditProjectTitleAndDescription(EditProjectTitleAndDescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditProjectTitleAndDescription();
            }

            ExecuteUserProjectCommand(new UpdateProjectTitleAndDescriptionCommand
            {
                ProjectId = ProjectId,
                Description = model.ProjectDescription,
                Title = model.Title
            });

            if (!ModelState.IsValid)
            {
                return EditProjectTitleAndDescription();
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