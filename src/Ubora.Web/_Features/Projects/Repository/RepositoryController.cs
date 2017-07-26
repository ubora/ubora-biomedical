using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Projects.Queries;
using Ubora.Domain.Projects.Repository;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Repository
{
    [ProjectRoute("[controller]")]
    public class RepositoryController : ProjectController
    {
        private readonly IUboraStorageProvider _uboraStorageProvider;

        public RepositoryController(IUboraStorageProvider uboraStorageProvider)
        {
            _uboraStorageProvider = uboraStorageProvider;
        }

        public IActionResult Repository()
        {
            var projectFiles = QueryProcessor.ExecuteQuery(new GetAvailableProjectFilesQuery(ProjectId));
            var model = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                Files = projectFiles.Select(x =>
                {
                    var fileViewModel = AutoMapper.Map<ProjectFileViewModel>(x);
                    return fileViewModel;
                }).ToList()
            };

            return View(nameof(Repository), model);
        }

        [Route("AddFile")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFile(AddFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Repository();
            }

            var filePath = model.ProjectFile.FileName.Replace(@"\", "/");
            var fileName = Path.GetFileName(filePath);

            var fileStream = model.ProjectFile.OpenReadStream();
            var blobLocation = BlobLocations.GetRepositoryFileBlobLocation(ProjectId, fileName);

            await _uboraStorageProvider.SavePrivateStreamToBlobAsync(blobLocation, fileStream);

            ExecuteUserProjectCommand(new AddFileCommand
            {
                Id = Guid.NewGuid(),
                BlobLocation = blobLocation,
                FileName = fileName,
            });

            if (!ModelState.IsValid)
            {
                return Repository();
            }

            return RedirectToAction(nameof(Repository));
        }

        [Route("HideFile")]
        [Authorize]
        public IActionResult HideFile(Guid fileid)
        {
            ExecuteUserProjectCommand(new HideFileCommand { Id = fileid });

            return RedirectToAction(nameof(Repository));
        }

        [Route("DownloadFile")]
        [Authorize]
        public IActionResult DownloadFile(Guid fileId)
        {
            var file = QueryProcessor.FindById<ProjectFile>(fileId);

            var blobSasUrl = _uboraStorageProvider.GetBlobSasUrl(file.Location, DateTime.UtcNow.AddSeconds(15));

            return Redirect(blobSasUrl);
        }
    }
}
