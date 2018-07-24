using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Repository.Commands;
using Ubora.Domain.Projects.Repository.Events;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Projects.Repository
{
    [ProjectRoute("[controller]")]
    [DisableProjectControllerAuthorization]
    [Authorize(Policy = nameof(Policies.CanViewProjectRepository))]
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

        public async Task<IActionResult> Repository()
        {
            var projectFiles = QueryProcessor.Find(new IsProjectFileSpec(ProjectId)
                    && !new IsHiddenFileSpec());

            var canHideProjectFile = await AuthorizationService.IsAuthorizedAsync(User, Policies.CanHideProjectFile);

            var model = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                AllFiles = projectFiles.GroupBy(file => file.FolderName,
                    file => _projecFileViewModelFactory.Create(file)),
                AddFileViewModel = new AddFileViewModel(),
                CanHideProjectFile = canHideProjectFile
            };

            return View(nameof(Repository), model);
        }

        [Route("AddFile")]
        [Authorize(Policy = nameof(Policies.CanAddFileRepository))]
        [HttpPost]
        public async Task<IActionResult> AddFile(AddFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }

            foreach (var file in model.ProjectFiles)
            {
                var fileName = file.GetFileName();
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
                }, Notice.Success(SuccessTexts.RepositoryFileAdded));

                if (!ModelState.IsValid)
                {
                    return ModelState.ToJsonResult();
                }
            }

            return Ok();
        }

        [Route("HideFile")]
        [Authorize(Policy = nameof(Policies.CanHideProjectFile))]
        public IActionResult HideFile(Guid fileid)
        {
            ExecuteUserProjectCommand(new HideFileCommand { Id = fileid }, Notice.Success(SuccessTexts.RepositoryFileHidden));

            return RedirectToAction(nameof(Repository));
        }

        [Route("UpdateFile")]
        [Authorize(Policy = nameof(Policies.CanUpdateFileRepository))]
        public IActionResult UpdateFile(Guid fileId)
        {
            var file = QueryProcessor.FindById<ProjectFile>(fileId);
            var model = AutoMapper.Map<UpdateFileViewModel>(file);

            return View(nameof(UpdateFile), model);
        }

        [Route("UpdateFile")]
        [Authorize(Policy = nameof(Policies.CanUpdateFileRepository))]
        [HttpPost]
        public async Task<IActionResult> UpdateFile(UpdateFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return UpdateFile(model.FileId);
            }

            var file = QueryProcessor.FindById<ProjectFile>(model.FileId);
            var fileName = model.ProjectFile.GetFileName();

            var blobLocation = BlobLocations.GetRepositoryFileBlobLocation(ProjectId, fileName);
            await SaveBlobAsync(model.ProjectFile, blobLocation);

            ExecuteUserProjectCommand(new UpdateFileCommand
            {
                Id = file.Id,
                FileName = model.ProjectFile.FileName,
                BlobLocation = blobLocation,
                FileSize = model.ProjectFile.Length,
                Comment = model.Comment
            }, Notice.Success(SuccessTexts.RepositoryFileUpdated));

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
                    EventId = x.Id,
                    FileName = ((UboraFileEvent)x.Data).FileName,
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

        [Route("DownloadFile")]
        public IActionResult DownloadFile(Guid fileId)
        {
            var file = QueryProcessor.FindById<ProjectFile>(fileId);
            if (file == null)
            {
                throw new InvalidOperationException();
            }

            var blobSasUrl = _uboraStorageProvider.GetReadUrl(file.Location, DateTime.UtcNow.AddSeconds(5));

            return Redirect(blobSasUrl);
        }

        [Route("View3DFile")]
        public IActionResult View3DFile(Guid fileId)
        {
            var file = QueryProcessor.FindById<ProjectFile>(fileId);
            if (file == null)
            {
                throw new InvalidOperationException();
            }

            var blobSasUrl = _uboraStorageProvider.GetReadUrl(file.Location, DateTime.UtcNow.AddSeconds(5));

            var model = new View3DFileViewModel
            {
                FileName = file.FileName,
                ProjectName = Project.Title,
                FileBlobSasUrl = blobSasUrl
            };

            return View(nameof(View3DFile), model);
        }

        [Route("DownloadHistoryFile")]
        public IActionResult DownloadHistoryFile(Guid eventId)
        {
            var fileEvent = _eventStreamQuery.FindFileEvent(ProjectId, eventId);
            if (fileEvent == null)
            {
                throw new InvalidOperationException();
            }

            var fileLocation = ((UboraFileEvent)fileEvent.Data).Location;

            var blobSasUrl = _uboraStorageProvider.GetReadUrl(fileLocation, DateTime.UtcNow.AddSeconds(5));

            return Redirect(blobSasUrl);
        }

        [Route("View3DHistoryFile")]
        public IActionResult View3DHistoryFile(Guid eventId)
        {
            var fileEvent = _eventStreamQuery.FindFileEvent(ProjectId, eventId);
            if (fileEvent == null)
            {
                throw new InvalidOperationException();
            }

            var fileLocation = ((UboraFileEvent)fileEvent.Data).Location;

            var blobSasUrl = _uboraStorageProvider.GetReadUrl(fileLocation, DateTime.UtcNow.AddSeconds(5));

            var file = QueryProcessor.FindById<ProjectFile>(((UboraFileEvent)fileEvent.Data).Id);

            var model = new View3DFileViewModel
            {
                FileName = file.FileName,
                ProjectName = Project.Title,
                FileBlobSasUrl = blobSasUrl
            };

            return View(nameof(View3DFile), model);
        }

        private async Task SaveBlobAsync(IFormFile projectFile, BlobLocation blobLocation)
        {
            var fileStream = projectFile.OpenReadStream();
            await _uboraStorageProvider.SavePrivate(blobLocation, fileStream);
        }
    }
}
