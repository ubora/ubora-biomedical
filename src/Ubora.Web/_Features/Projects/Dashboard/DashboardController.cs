using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Projects.Dashboard
{
    [ProjectRoute("[controller]")]
    public class DashboardController : ProjectController
    {
        private readonly IStorageProvider _storageProvider;
        private readonly ImageResizer _imageResizer;

        public DashboardController(
            IStorageProvider storageProvider,
            ImageResizer imageResizer)
        {
            _storageProvider = storageProvider;
            _imageResizer = imageResizer;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Dashboard()
        {
            var model = AutoMapper.Map<ProjectDashboardViewModel>(Project);
            model.IsProjectMember = await AuthorizationService.AuthorizeAsync(User, null, new IsProjectMemberRequirement());
            model.ImagePath = _storageProvider.GetDefaultOrBlobUrl(Project, 1500, 300);
            model.HasImage = Project.ProjectImageLastUpdated != null;

            return View(nameof(Dashboard), model);
        }

        [Route(nameof(EditProjectDescription))]
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
        public IActionResult EditProjectImage()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(EditProjectImage))]
        public async Task<IActionResult> EditProjectImage(EditProjectImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditProjectImage();
            }

            var imageStream = model.Image.OpenReadStream();

            var containerName = BlobLocation.ContainerNames.Projects;
            var blobLocation = $"{ProjectId}/project-image/";

            await _imageResizer.SaveAsJpegAsync(new BlobLocation(containerName, blobLocation + "original.jpg"), imageStream);
            await _imageResizer.CreateResizedImageAndSaveAsJpegAsync(new BlobLocation(containerName, blobLocation + "400x150.jpg"), imageStream, 400, 150);
            await _imageResizer.CreateResizedImageAndSaveAsJpegAsync(new BlobLocation(containerName, blobLocation + "1500x300.jpg"), imageStream, 1500, 300);

            ExecuteUserProjectCommand(new UpdateProjectImageCommand
            {
                ProjectId = ProjectId,
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

            var containerName = BlobLocation.ContainerNames.Projects;
            var blobLocation = $"{ProjectId}/project-image/";

            await _storageProvider.DeleteBlobAsync(containerName, blobLocation + "original.jpg");
            await _storageProvider.DeleteBlobAsync(containerName, blobLocation + "400x150.jpg");
            await _storageProvider.DeleteBlobAsync(containerName, blobLocation + "1500x300.jpg");

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