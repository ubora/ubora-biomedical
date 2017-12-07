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
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web._Features.Projects._Shared;

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
                CanOpenWorkpackageThree = isAuthorizedToOpenWp3 && !isWp3Opened
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
            });

            if (!ModelState.IsValid)
            {
                return AddCandidate();
            }

            return RedirectToAction(nameof(Voting), "Candidates");
        }

        public async Task<IActionResult> Candidate(Guid candidateId, [FromServices]CandidateViewModel.Factory candidateViewModelFactory)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var model = await candidateViewModelFactory.Create(candidate, User);

            return View(nameof(Candidate), model);
        }

        [Authorize(Policies.CanEditProjectCandidate)]
        public IActionResult EditCandidate(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var model = AutoMapper.Map<EditCandidateViewModel>(candidate);

            return View(nameof(EditCandidate), model);
        }

        [HttpPost]
        [Authorize(Policies.CanEditProjectCandidate)]
        public IActionResult EditCandidate(EditCandidateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditCandidate(model.Id);
            }

            ExecuteUserProjectCommand(new EditCandidateCommand
            {
                Description = model.Description,
                Title = model.Title,
                Id = model.Id
            });

            if (!ModelState.IsValid)
            {
                return EditCandidate(model.Id);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.Id });
        }

        [Authorize(Policies.CanChangeProjectCandidateImage)]
        public IActionResult EditCandidateImage(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var model = AutoMapper.Map<EditCandidateImageViewModel>(candidate);

            return View(nameof(EditCandidateImage), model);
        }

        [HttpPost]
        [Authorize(Policies.CanChangeProjectCandidateImage)]
        public async Task<IActionResult> EditCandidateImage(EditCandidateImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditCandidateImage(model.Id);
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
                return EditCandidateImage(model.Id);
            }

            return RedirectToAction(nameof(Candidate), new { candidateId = model.Id });
        }

        [Authorize(Policies.CanRemoveProjectCandidateImage)]
        public IActionResult RemoveCandidateImage(Guid candidateId)
        {
            var candidate = QueryProcessor.FindById<Candidate>(candidateId);
            var model = AutoMapper.Map<RemoveCandidateImageViewModel>(candidate);

            return View(nameof(RemoveCandidateImage), model);
        }

        [HttpPost]
        [Authorize(Policies.CanRemoveProjectCandidateImage)]
        public async Task<IActionResult> RemoveCandidateImage(RemoveCandidateImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RemoveCandidateImage(model.Id);
            }

            await _imageStorageProvider.DeleteImagesAsync(BlobLocations.GetProjectCandidateBlobLocation(ProjectId, model.Id));

            ExecuteUserProjectCommand(new DeleteCandidateImageCommand
            {
                CandidateId = model.Id,
                Actor = UserInfo
            });

            if (!ModelState.IsValid)
            {
                return RemoveCandidateImage(model.Id);
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
            });

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
            });

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
            });

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
            });

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
            ExecuteUserProjectCommand(new OpenWorkpackageThreeCommand());

            if (!ModelState.IsValid)
            {
                Notices.Error("Failed to open work package 3!");
                return await Voting(candidateItemViewModelFactory);
            }

            Notices.Success("Work package 3 opened successfully!");

            return RedirectToAction(nameof(Voting));
        }
    }
}
