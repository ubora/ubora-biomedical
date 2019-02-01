using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Dashboard
{
    [ProjectRoute("")]
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

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> Dashboard([FromServices]ProjectDashboardViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(Project, User);
            model.DescriptionHtml = await ConvertQuillDeltaToHtml(Project.DescriptionV2);

            return View(nameof(Dashboard), model);
        }

        [HttpGet("edit-project")]
        [Authorize(Policies.CanEditProjectTitleAndDescription)]
        public async Task<IActionResult> EditProjectTitleAndDescription()
        {
            var editProjectDescription = new EditProjectTitleAndDescriptionViewModel
            {
                DescriptionQuillDelta = await SanitizeQuillDeltaForEditing(Project.DescriptionV2),
                Title = Project.Title
            };

            return View(nameof(EditProjectTitleAndDescription), editProjectDescription);
        }

        [HttpPost("edit-project")]
        [Authorize(Policies.CanEditProjectTitleAndDescription)]
        public async Task<IActionResult> EditProjectTitleAndDescription(EditProjectTitleAndDescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await EditProjectTitleAndDescription();
            }

            ExecuteUserProjectCommand(new UpdateProjectTitleAndDescriptionCommand
            {
                ProjectId = ProjectId,
                Description = new QuillDelta(model.DescriptionQuillDelta),
                Title = model.Title
            }, Notice.Success(SuccessTexts.ProjectTitleAndDescriptionUpdated));

            if (!ModelState.IsValid)
            {
                return await EditProjectTitleAndDescription();
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet("change-image")]
        [Authorize(Policies.CanChangeProjectImage)]
        public IActionResult EditProjectImage()
        {
            return View();
        }

        [HttpPost("change-image")]
        [Authorize(Policies.CanChangeProjectImage)]
        public async Task<IActionResult> EditProjectImage(EditProjectImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditProjectImage();
            }

            using (var imageStream = model.Image.OpenReadStream())
            {
                var blobLocation = BlobLocations.GetProjectImageBlobLocation(ProjectId);
                await _imageStorage.SaveImageAsync(imageStream, blobLocation, SizeOptions.AllDefaultSizes);

                ExecuteUserProjectCommand(new UpdateProjectImageCommand
                {
                    ProjectId = ProjectId,
                    BlobLocation = blobLocation,
                    Actor = UserInfo
                }, Notice.Success(SuccessTexts.ProjectImageUploaded));
            }

            if (!ModelState.IsValid)
            {
                return EditProjectImage();
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet("remove-image")]
        public IActionResult RemoveProjectImage()
        {
            var model = new RemoveProjectImageViewModel
            {
                Title = Project.Title
            };

            return View(model);
        }

        [HttpPost("remove-image")]
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
            }, Notice.Success(SuccessTexts.ProjectImageDeleted));

            if (!ModelState.IsValid)
            {
                return RemoveProjectImage();
            }

            return RedirectToAction(nameof(Dashboard));
        }
    }
}