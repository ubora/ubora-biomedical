using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates.Commands;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class ConceptualDesignController : ProjectController
    {
        private readonly IUboraStorageProvider _uboraStorageProvider;

        public ConceptualDesignController(IUboraStorageProvider uboraStorageProvider)
        {
            _uboraStorageProvider = uboraStorageProvider;
        }


        public IActionResult AddCandidate()
        {
            var model = new AddCandidateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCandidate(AddCandidateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return AddCandidate();
            }

            var fileName = GetFileName(model.Image);
            var blobLocation = BlobLocations.GetRepositoryFileBlobLocation(ProjectId, fileName);
            await SaveBlobAsync(model.Image, blobLocation);

            ExecuteUserProjectCommand(new AddCandidateCommand
            {
                Id = Guid.NewGuid(),
                Title = model.Name,
                Description = model.Description,
                ImageLocation = blobLocation
            });

            if (!ModelState.IsValid)
            {
                return AddCandidate();
            }

            return RedirectToAction("ConceptualDesign", "WorkpackageTwo");
        }

        public IActionResult Candidate(Guid candidateId)
        {
            return View();
        }

        private async Task SaveBlobAsync(IFormFile file, BlobLocation blobLocation)
        {
            var fileStream = file.OpenReadStream();
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
