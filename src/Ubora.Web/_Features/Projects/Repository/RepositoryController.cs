using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using TwentyTwenty.Storage;
using Ubora.Domain.Projects.Repository;

namespace Ubora.Web._Features.Projects.Repository
{
    [ProjectRoute("[controller]")]
    public class RepositoryController : ProjectController
    {
        private readonly IStorageProvider _storageProvider;

        public RepositoryController(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public IActionResult Repository()
        {
            var projectFiles = QueryProcessor.Find<ProjectFile>().Where(x => x.ProjectId == ProjectId);

            var model = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                Files = projectFiles.Select(x =>
                {
                    var fileViewModel = AutoMapper.Map<ProjectFileViewModel>(x);
                    fileViewModel.FileLocation = _storageProvider.GetBlobUrl(x.Location.ContainerName, x.Location.BlobName);
                    return fileViewModel;
                }).ToList()
            };

            return View(nameof(Repository), model);
        }

        [HttpPost]
        [Authorize]
        [Route(nameof(AddFile))]
        public IActionResult AddFile(AddFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Repository();
            }

            var filePath = model.ProjectFile.FileName.Replace(@"\", "/");
            var fileName = Path.GetFileName(filePath);

            ExecuteUserProjectCommand(new AddFileCommand
            {
                Id = Guid.NewGuid(),
                Stream = model.ProjectFile.OpenReadStream(),
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
