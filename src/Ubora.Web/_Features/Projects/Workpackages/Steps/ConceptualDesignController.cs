using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Commands;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class ConceptualDesignController : ProjectController
    {
        private readonly ImageStorageProvider _imageStorageProvider;

        public ConceptualDesignController(ImageStorageProvider imageStorageProvider)
        {
            _imageStorageProvider = imageStorageProvider;
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
            var candidateId = Guid.NewGuid();
            BlobLocation blobLocation = null;
            if(model.Image != null)
            {
                blobLocation = BlobLocations.GetProjectCandidateBlobLocation(candidateId);
                var imageStream = model.Image.OpenReadStream();
                await _imageStorageProvider.SaveImageAsync(imageStream, blobLocation, SizeOptions.AllDefaultSizes);
            }

            ExecuteUserProjectCommand(new AddCandidateCommand
            {
                Id = candidateId,
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
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var model = AutoMapper.Map<CandidateViewModel>(candidate);
            model.ImageUrl = _imageStorageProvider.GetDefaultOrBlobImageUrl(candidate.ImageLocation, ImageSize.Banner1500x300);

            return View(model);
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
