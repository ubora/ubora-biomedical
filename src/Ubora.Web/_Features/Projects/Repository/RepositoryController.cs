using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Repository.Commands;
using Ubora.Domain.Projects.Repository.Events;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Repository
{
    [ProjectRoute("[controller]")]
    public class RepositoryController : ProjectController
    {
        private readonly IUboraStorageProvider _uboraStorageProvider;
        private readonly IEventStreamQuery _eventStreamQuery;
        private readonly ProjectFileViewModel.Factory _projecFileViewModelFactory;

        public RepositoryController(IUboraStorageProvider uboraStorageProvider,
            IEventStreamQuery eventStreamQuery,
            ProjectFileViewModel.Factory projectFileViewModelFactory)
        {
            _uboraStorageProvider = uboraStorageProvider;
            _eventStreamQuery = eventStreamQuery;
            _projecFileViewModelFactory = projectFileViewModelFactory;
        }

        public IActionResult Repository()
        {
            var projectFiles = QueryProcessor.Find(new IsProjectFileSpec(ProjectId)
                    && !new IsHiddenFileSpec());

            var isProjectLeader = Project.Members.Any(x => x.UserId == UserId && x.IsLeader);

            var model = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                AllFiles = projectFiles.GroupBy(file => file.FolderName,
                    file => _projecFileViewModelFactory.Create(file)),
                AddFileViewModel = new AddFileViewModel(),
                IsProjectLeader = isProjectLeader
            };

            return View(nameof(Repository), model);
        }

        [Route("AddFile")]
        [HttpPost]
        public async Task<IActionResult> AddFile(AddFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("AddFilePartial", model);
            }

            foreach (var file in model.ProjectFiles)
            {
                var fileName = GetFileName(file);
                var blobLocation = BlobLocations.GetRepositoryFileBlobLocation(ProjectId, fileName);
                await SaveBlobAsync(file, blobLocation);

                ExecuteUserProjectCommand(new AddFileCommand
                {
                    Id = Guid.NewGuid(),
                    BlobLocation = blobLocation,
                    FileName = fileName,
                    FileSize = file.Length,
                    Comment = model.Comment,
                    FolderName = model.FolderName
                });

                if (!ModelState.IsValid)
                {
                    return PartialView("AddFilePartial", model);
                }
            }

            return Ok();
        }


        [Route("HideFile")]
        [Authorize(Policy = nameof(Policies.CanHideProjectFile))]
        public IActionResult HideFile(Guid fileid)
        {
            ExecuteUserProjectCommand(new HideFileCommand { Id = fileid });

            return RedirectToAction(nameof(Repository));
        }

        [Route("UpdateFile")]
        public IActionResult UpdateFile(Guid fileId)
        {
            var file = QueryProcessor.FindById<ProjectFile>(fileId);
            var model = AutoMapper.Map<UpdateFileViewModel>(file);

            return View(nameof(UpdateFile), model);
        }

        [Route("UpdateFile")]
        [HttpPost]
        public async Task<IActionResult> UpdateFile(UpdateFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return UpdateFile(model.FileId);
            }

            var file = QueryProcessor.FindById<ProjectFile>(model.FileId);
            var fileName = GetFileName(model.ProjectFile);

            var blobLocation = BlobLocations.GetRepositoryFileBlobLocation(ProjectId, fileName);
            await SaveBlobAsync(model.ProjectFile, blobLocation);

            ExecuteUserProjectCommand(new UpdateFileCommand
            {
                Id = file.Id,
                BlobLocation = blobLocation,
                FileSize = model.ProjectFile.Length,
                Comment = model.Comment
            });

            if (!ModelState.IsValid)
            {
                return UpdateFile(model.FileId);
            }

            return RedirectToAction(nameof(Repository));
        }

        [Route("FileHistory")]
        public IActionResult FileHistory(Guid fileId)
        {
            var fileEvents = _eventStreamQuery.FindFileEvents(ProjectId, fileId);

            var allFiles = fileEvents
                .Select(x => new FileItemHistoryViewModel
                {
                    DownloadUrl = _uboraStorageProvider.GetReadUrl(((UboraFileEvent)x.Data).Location, DateTime.UtcNow.AddSeconds(15)),
                    FileSize = ((UboraFileEvent)x.Data).FileSize,
                    RevisionNumber = ((UboraFileEvent)x.Data).RevisionNumber,
                    FileAddedOn = x.Timestamp,
                    Comment = ((UboraFileEvent)x.Data).Comment
                });

            var file = QueryProcessor.FindById<ProjectFile>(fileId);

            var model = new FileHistoryViewModel
            {
                FileName = file.FileName,
                ProjectName = Project.Title,
                Files = allFiles.OrderByDescending(x => x.FileAddedOn)
            };
            return View(nameof(FileHistory), model);
        }

        private async Task SaveBlobAsync(IFormFile projectFile, BlobLocation blobLocation)
        {
            var fileStream = projectFile.OpenReadStream();
            await _uboraStorageProvider.SavePrivate(blobLocation, fileStream);
        }

        private string GetFileName(IFormFile projectFile)
        {
            if (projectFile != null)
            {
                var filePath = projectFile.FileName.Replace(@"\", "/");
                var fileName = Path.GetFileName(filePath);
                return fileName;
            }

            return "";
        }
    }
}
