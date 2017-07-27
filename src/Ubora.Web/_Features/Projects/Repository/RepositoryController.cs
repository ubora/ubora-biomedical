using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
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
                }).ToList(),
                AddFileViewModel = new AddFileViewModel
                {
                    ActionName = nameof(AddFile)
                }
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

            var blobLocation = GetBlobLocation(model);
            await SaveBlobAsync(model, blobLocation);

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

        [Route("UpdateFile")]
        [Authorize]
        public IActionResult UpdateFile(Guid fileId)
        {
            var file = QueryProcessor.FindById<ProjectFile>(fileId);
            var model = AutoMapper.Map<UpdateFileViewModel>(file);
            model.AddFileViewModel = new AddFileViewModel
            {
                ActionName = nameof(UpdateFile),
                FileId = file.Id
            };

            return View(nameof(UpdateFile), model);
        }

        [Route("UpdateFile")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateFile(AddFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return UpdateFile(model.FileId);
            }

            var blobLocation = GetBlobLocation(model);
            await SaveBlobAsync(model, blobLocation);

            var file = QueryProcessor.FindById<ProjectFile>(model.FileId);

            ExecuteUserProjectCommand(new UpdateFileCommand
            {
                Id = file.Id,
                BlobLocation = blobLocation,
            });

            if (!ModelState.IsValid)
            {
                return UpdateFile(model.FileId);
            }

            return RedirectToAction(nameof(Repository));
        }

        private BlobLocation GetBlobLocation(AddFileViewModel model)
        {
            var filePath = model.ProjectFile.FileName.Replace(@"\", "/");
            var fileName = Path.GetFileName(filePath);
            var blobLocation = BlobLocations.GetRepositoryFileBlobLocation(ProjectId, fileName);

            return blobLocation;
        }

        private async Task SaveBlobAsync(AddFileViewModel model, BlobLocation blobLocation)
        {
            var fileStream = model.ProjectFile.OpenReadStream();
            await _uboraStorageProvider.SavePrivateStreamToBlobAsync(blobLocation, fileStream);
        }
    }
}
