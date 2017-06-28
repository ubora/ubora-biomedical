using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository;

namespace Ubora.Web._Features.Projects.Repository
{
    public class RepositoryController : ProjectController
    {
        private readonly IMapper _mapper;

        public RepositoryController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

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

            var filePath = model.ProjectFile.FileName.Replace(@"\\", "/");
            var fileName = Path.GetFileName(filePath);

            ExecuteUserProjectCommand(new AddFileCommand
            {
                Id = Guid.NewGuid(),
                Stream = model.ProjectFile.OpenReadStream(),
                FileName = fileName
            });

            if (!ModelState.IsValid)
            {
                return Repository();
            }

            return RedirectToAction(nameof(Repository));
        }

        public IActionResult DownloadFile(Guid id)
        {
            var file = FindById<ProjectFile>(id);

            return File(file.FileLocation, "application/octet-stream", file.FileName);
        }
    }
}
