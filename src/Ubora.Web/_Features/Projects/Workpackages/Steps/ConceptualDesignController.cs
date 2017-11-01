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
                blobLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, candidateId);
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

            return RedirectToAction("Voting", "WorkpackageTwo");
        }

        public IActionResult Candidate(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var model = AutoMapper.Map<CandidateViewModel>(candidate);
            model.ImageUrl = _imageStorageProvider.GetDefaultOrBlobImageUrl(candidate.ImageLocation, ImageSize.Banner1500x1125);

            return View(model);
        }

        public IActionResult EditCandidate(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var model = AutoMapper.Map<EditCandidateViewModel>(candidate);

            return View(model);
        }

        [HttpPost]
        public IActionResult EditCandidate(EditCandidateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditCandidate(model);
            }

            ExecuteUserProjectCommand(new EditCandidateCommand
            {
                Description = model.Description,
                Title = model.Title,
                Id = model.Id
            });

            if (!ModelState.IsValid)
            {
                return EditCandidate(model);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.Id});
        }

        public IActionResult EditCandidateImage(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var model = AutoMapper.Map<EditCandidateImageViewModel>(candidate);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCandidateImage(EditCandidateImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await EditCandidateImage(model);
            }

            var imageLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, model.Id);
            var imageStream = model.Image.OpenReadStream();
            await _imageStorageProvider.SaveImageAsync(imageStream, imageLocation, SizeOptions.AllDefaultSizes);

            ExecuteUserProjectCommand(new EditCandidateImageCommand
            {
                Id = model.Id,
                ImageLocation = imageLocation
            });

            if (!ModelState.IsValid)
            {
                return await EditCandidateImage(model);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.Id });
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
