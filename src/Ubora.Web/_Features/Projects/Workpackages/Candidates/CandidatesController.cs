using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Commands;
using Ubora.Domain.Projects.Candidates.Specifications;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects.Workpackages.Queries;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web._Features.Projects._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Candidates
{
    public class CandidatesController : ProjectController
    {
        private readonly ImageStorageProvider _imageStorageProvider;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Title"] = "Voting";
            ViewData["MenuOption"] = ProjectMenuOption.Workpackages;
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.Voting;
        }

        public CandidatesController(ImageStorageProvider imageStorageProvider)
        {
            _imageStorageProvider = imageStorageProvider;
        }

        [Route(nameof(Voting))]
        public async Task<IActionResult> Voting([FromServices] CandidateItemViewModel.Factory candidateItemViewModelFactory)
        {
            var candidates = QueryProcessor.Find(new IsProjectCandidateSpec(ProjectId));

            var isAuthorizedToOpenWp3 = await AuthorizationService.IsAuthorizedAsync(User, Policies.CanOpenWorkpackageThree);
            var isWp3Opened = QueryProcessor.ExecuteQuery(new IsWorkpackageThreeOpenedQuery(ProjectId));

            var candidateViewModels = candidates.Select(candidateItemViewModelFactory.Create);
            var model = new VotingViewModel
            {
                Candidates = candidateViewModels,
                CanOpenWorkpackageThree = isAuthorizedToOpenWp3 && !isWp3Opened && candidates.Any() // Untested
            };

            return View(nameof(Voting), model);
        }

        [Authorize(Policies.CanAddProjectCandidate)]
        public IActionResult AddCandidate()
        {
            var model = new AddCandidateViewModel();
            return View(nameof(AddCandidate), model);
        }

        [HttpPost]
        [Authorize(Policies.CanAddProjectCandidate)]
        public async Task<IActionResult> AddCandidate(AddCandidateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return AddCandidate();
            }

            var fileName = model.Image.GetFileName();
            var candidateId = Guid.NewGuid();
            BlobLocation blobLocation = null;
            if (model.Image != null)
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
            }, Notice.Success(SuccessTexts.CandidateAdded));

            if (!ModelState.IsValid)
            {
                return AddCandidate();
            }

            return RedirectToAction(nameof(Voting), "Candidates");
        }

        public async Task<IActionResult> RemoveCandidate(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var canRemoveCandidate = (await AuthorizationService.AuthorizeAsync(User, candidate, Policies.CanRemoveCandidate)).Succeeded;
            if (!canRemoveCandidate)
            {
                return Forbid();
            }

            var model = new RemoveCandidateViewModel
            {
                CandidateId = candidateId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCandidate(RemoveCandidateViewModel model)
        {
            var candidate = QueryProcessor.FindById<Candidate>(model.CandidateId);
            var canRemoveCandidate = (await AuthorizationService.AuthorizeAsync(User, candidate, Policies.CanRemoveCandidate)).Succeeded;
            if (!canRemoveCandidate)
            {
                return Forbid();
            }

            ExecuteUserProjectCommand(new RemoveCandidateCommand
            {
                CandidateId = model.CandidateId
            }, Notice.Success(SuccessTexts.CandidateRemoved));

            return RedirectToAction(nameof(Voting), "Candidates");
        }

        public async Task<IActionResult> Candidate(Guid candidateId, [FromServices]CandidateViewModel.Factory candidateViewModelFactory)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var model = await candidateViewModelFactory.Create(candidate, User);

            return View(nameof(Candidate), model);
        }

        
        public async Task<IActionResult> EditCandidate(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var canEditProjectCandidate = (await AuthorizationService.AuthorizeAsync(User, candidate, Policies.CanEditProjectCandidate)).Succeeded;
            if (!canEditProjectCandidate)
            {
                return Forbid();
            }

            var model = AutoMapper.Map<EditCandidateViewModel>(candidate);

            return View(nameof(EditCandidate), model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCandidate(EditCandidateViewModel model)
        {
            var candidate = QueryProcessor.FindById<Candidate>(model.Id);
            var canEditProjectCandidate = (await AuthorizationService.AuthorizeAsync(User, candidate, Policies.CanEditProjectCandidate)).Succeeded;
            if (!canEditProjectCandidate)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return await EditCandidate(model.Id);
            }

            ExecuteUserProjectCommand(new EditCandidateCommand
            {
                Description = model.Description,
                Title = model.Title,
                Id = model.Id
            }, Notice.Success(SuccessTexts.CandidateEdited));

            if (!ModelState.IsValid)
            {
                return await EditCandidate(model.Id);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.Id });
        }

        public async Task<IActionResult> EditCandidateImage(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var canChangeProjectCandidateImage = (await AuthorizationService.AuthorizeAsync(User, candidate, Policies.CanChangeProjectCandidateImage)).Succeeded;
            if (!canChangeProjectCandidateImage)
            {
                return Forbid();
            }

            var model = AutoMapper.Map<EditCandidateImageViewModel>(candidate);

            return View(nameof(EditCandidateImage), model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCandidateImage(EditCandidateImageViewModel model)
        {
            var candidate = QueryProcessor.FindById<Candidate>(model.Id);
            var canChangeProjectCandidateImage = (await AuthorizationService.AuthorizeAsync(User, candidate, Policies.CanChangeProjectCandidateImage)).Succeeded;
            if (!canChangeProjectCandidateImage)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return await EditCandidateImage(model.Id);
            }

            var imageLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, model.Id);
            var imageStream = model.Image.OpenReadStream();
            await _imageStorageProvider.SaveImageAsync(imageStream, imageLocation, SizeOptions.AllDefaultSizes);

            ExecuteUserProjectCommand(new EditCandidateImageCommand
            {
                Id = model.Id,
                ImageLocation = imageLocation
            }, Notice.Success(SuccessTexts.CandidateImageUploaded));

            if (!ModelState.IsValid)
            {
                return await EditCandidateImage(model.Id);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.Id });
        }

        public async Task<IActionResult> RemoveCandidateImage(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var canChangeProjectCandidateImage = (await AuthorizationService.AuthorizeAsync(User, candidate, Policies.CanRemoveProjectCandidateImage)).Succeeded;
            if (!canChangeProjectCandidateImage)
            {
                return Forbid();
            }
            var model = AutoMapper.Map<RemoveCandidateImageViewModel>(candidate);

            return View(nameof(RemoveCandidateImage), model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCandidateImage(RemoveCandidateImageViewModel model)
        {
            var candidate = QueryProcessor.FindById<Candidate>(model.Id);
            var canChangeProjectCandidateImage = (await AuthorizationService.AuthorizeAsync(User, candidate, Policies.CanRemoveProjectCandidateImage)).Succeeded;
            if (!canChangeProjectCandidateImage)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return await RemoveCandidateImage(model.Id);
            }

            await _imageStorageProvider.DeleteImagesAsync(BlobLocations.GetProjectCandidateBlobLocation(ProjectId, model.Id));

            ExecuteUserProjectCommand(new DeleteCandidateImageCommand
            {
                CandidateId = model.Id,
                Actor = UserInfo
            }, Notice.Success(SuccessTexts.CandidateImageDeleted));

            if (!ModelState.IsValid)
            {
                return await RemoveCandidateImage(model.Id);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentViewModel model, [FromServices] CandidateViewModel.Factory candidateViewModelFactory)
        {
            if(!ModelState.IsValid)
            {
                return await Candidate(model.CandidateId, candidateViewModelFactory);
            }

            ExecuteUserProjectCommand(new AddCandidateCommentCommand
            {
                CandidateId = model.CandidateId,
                CommentText = model.CommentText,
            }, Notice.Success(SuccessTexts.CandidateCommentAdded));

            if (!ModelState.IsValid)
            {
                return await Candidate(model.CandidateId, candidateViewModelFactory);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.CandidateId });
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(EditCommentViewModel model, [FromServices] CandidateViewModel.Factory candidateViewModelFactory)
        {
            if (!ModelState.IsValid)
            {
                return await Candidate(model.CandidateId, candidateViewModelFactory);
            }

            var candidate = QueryProcessor.FindById<Candidate>(model.CandidateId);
            var comment = candidate.Comments.Single(x => x.Id == model.Id);
            var canEditComment = (await AuthorizationService.AuthorizeAsync(User, comment, Policies.CanEditComment)).Succeeded;
            if(!canEditComment)
            {
                return Forbid();
            }

            ExecuteUserProjectCommand(new EditCandidateCommentCommand
            {
                CandidateId = model.CandidateId,
                CommentText = model.CommentText,
                CommentId = model.Id
            }, Notice.Success(SuccessTexts.CandidateCommentEdited));

            if (!ModelState.IsValid)
            {
                return await Candidate(model.CandidateId, candidateViewModelFactory);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.CandidateId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveComment(Guid candidateId, Guid commentId, [FromServices] CandidateViewModel.Factory candidateViewModelFactory)
        {
            if (!ModelState.IsValid)
            {
                return await Candidate(candidateId, candidateViewModelFactory);
            }

            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var comment = candidate.Comments.Single(x => x.Id == commentId);
            var canEditComment = (await AuthorizationService.AuthorizeAsync(User, comment, Policies.CanEditComment)).Succeeded;
            if (!canEditComment)
            {
                return Forbid();
            }

            ExecuteUserProjectCommand(new RemoveCandidateCommentCommand
            {
                CandidateId = candidateId,
                CommentId = commentId
            }, Notice.Success(SuccessTexts.CandidateCommentRemoved));

            if (!ModelState.IsValid)
            {
                return await Candidate(candidateId, candidateViewModelFactory);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = candidateId });
        }

        [HttpPost]
        public async Task<IActionResult> AddVote(AddVoteViewModel model, [FromServices] CandidateViewModel.Factory candidateViewModelFactory)
        {
            if (!ModelState.IsValid)
            {
                return await Candidate(model.CandidateId, candidateViewModelFactory);
            }

            var candidate = QueryProcessor.FindById<Candidate>(model.CandidateId);
            var canVoteForCandidate = await AuthorizationService.IsAuthorizedAsync(User, candidate, Policies.CanVoteCandidate);
            if (!canVoteForCandidate)
            {
                return Forbid();
            }

            ExecuteUserProjectCommand(new AddCandidateVoteCommand
            {
                CandidateId = model.CandidateId,
                Functionality = model.Functionality,
                Safety = model.Safety,
                Performance = model.Performace,
                Usability = model.Usability
            }, Notice.Success(SuccessTexts.CandidateVoteAdded));

            if (!ModelState.IsValid)
            {
                return await Candidate(model.CandidateId, candidateViewModelFactory);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.CandidateId });
        }

        [HttpPost]
        [Authorize(Policies.CanOpenWorkpackageThree)]
        public async Task<IActionResult> OpenWorkpackageThree([FromServices] CandidateItemViewModel.Factory candidateItemViewModelFactory)
        {
            ExecuteUserProjectCommand(new OpenWorkpackageThreeCommand(), Notice.Success(SuccessTexts.WP3Opened));

            if (!ModelState.IsValid)
            {
                Notices.NotifyOfError("Failed to open work package 3!");
                return await Voting(candidateItemViewModelFactory);
            }
            
            ExecuteUserProjectCommand(new OpenWorkpackageFourCommand(), Notice.Success("WP3 opened"));
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                
                Notices.NotifyOfError(message);
                return await Voting(candidateItemViewModelFactory);
            }

            return RedirectToAction(nameof(Voting));
        }
    }
}
