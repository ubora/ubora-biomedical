using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Projects.Repository;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Repository
{
    [ProjectRoute("[controller]")]
    public class RepositoryController : ProjectController
    {
        private readonly IStorageProvider _storageProvider;
        private readonly IUboraStorageProvider _uboraStorageProvider;

        public RepositoryController(IStorageProvider storageProvider, IUboraStorageProvider uboraStorageProvider)
        {
            _storageProvider = storageProvider;
            _uboraStorageProvider = uboraStorageProvider;
        }

        public IActionResult Repository()
        {
            var projectFiles = QueryProcessor.Find<ProjectFile>().Where(x => x.ProjectId == ProjectId);
            var blobExpiration = DateTime.UtcNow.AddHours(12);
            var model = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                Files = projectFiles.Select(x =>
                {
                    var fileViewModel = AutoMapper.Map<ProjectFileViewModel>(x);
                    fileViewModel.FileLocation = _storageProvider.GetBlobSasUrl(x.Location.ContainerName, x.Location.BlobPath, blobExpiration);
                    return fileViewModel;
                }).ToList()
            };

            return View(nameof(Repository), model);
        }

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
    }
}
