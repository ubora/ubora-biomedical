using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository;

namespace Ubora.Web._Features.Projects.Repository
{
    public class RepositoryController : ProjectController
    {
        private readonly IStorageProvider _storageProvider;
        private readonly IMapper _mapper;

        public RepositoryController(ICommandQueryProcessor processor, IStorageProvider storageProvider, IMapper mapper) : base(processor)
        {
            _storageProvider = storageProvider;
            _mapper = mapper;
        }

        [Route(nameof(Repository))]
        public IActionResult Repository()
        {
            var projectFiles = Find<ProjectFile>().Where(x => x.ProjectId == ProjectId);

            var repositoryFiles = new List<ProjectFileViewModel>();
            foreach (var file in projectFiles)
            {
                var repositoryFile = _mapper.Map<ProjectFileViewModel>(file);
                repositoryFiles.Add(repositoryFile);
            }

            var model = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                Files = repositoryFiles
            };
            return View(nameof(Repository), model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddFile(AddFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Repository();
            }

            var getCorrectFileName = model.ProjectFile.FileName.Substring(model.ProjectFile.FileName.LastIndexOf(@"\") + 1);

            ExecuteUserProjectCommand(new AddFileCommand
            {
                Id = Guid.NewGuid(),
                Stream = model.ProjectFile.OpenReadStream(),
                FileName = getCorrectFileName
            });

            return Repository();
        }

        public IActionResult DownloadFile(Guid id)
        {
            var file = FindById<ProjectFile>(id);

            return File(file.FileLocation, "application/octet-stream", file.FileName);
        }
    }
}
