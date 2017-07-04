﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository;

namespace Ubora.Web._Features.Projects.Repository
{
    public class RepositoryController : ProjectController
    {
        private readonly IMapper _mapper;
        private readonly IStorageProvider _storageProvider;

        public RepositoryController(ICommandQueryProcessor processor, IMapper mapper, IStorageProvider storageProvider) : base(processor)
        {
            _mapper = mapper;
            _storageProvider = storageProvider;
        }

        public IActionResult Repository()
        {
            var projectFiles = Find<ProjectFile>().Where(x => x.ProjectId == ProjectId);

            var model = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                Files = projectFiles.Select(x =>
                {
                    var fileViewModel =_mapper.Map<ProjectFileViewModel>(x);
                    fileViewModel.FileLocation = _storageProvider.GetBlobUrl(x.Location.ContainerName, x.Location.BlobName);
                    return fileViewModel;
                }).ToList()
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
