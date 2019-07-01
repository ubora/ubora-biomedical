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
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Web._Components.Discussions.Models;
using Ubora.Domain.Projects;
using System.Collections.Generic;
using System.Collections.Immutable;
using Ubora.Domain.Discussions;
using Ubora.Domain.Discussions.Commands;

namespace Ubora.Web._Features.Projects.Workpackages.Candidates
{
    [ProjectRoute("candidates")]
    public class CandidatesController : ProjectController
    {
        private readonly ImageStorageProvider _imageStorageProvider;

        public Guid CandidateId => RouteData.Values["CandidateId"] != null ? Guid.Parse((string)RouteData.Values["CandidateId"]) : Guid.Empty;
        public Candidate CurrentCandidate { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (CandidateId != default(Guid))
            {
                CurrentCandidate = QueryProcessor.FindById<Candidate>(CandidateId);
            }

            ViewData[nameof(PageTitle)] = "Voting";
            ViewData[nameof(ProjectMenuOption)] = ProjectMenuOption.Workpackages;
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.Voting;
        }

        public CandidatesController(ImageStorageProvider imageStorageProvider)
        {
            _imageStorageProvider = imageStorageProvider;
        }

        [HttpGet("")]
        public async Task<IActionResult> Voting([FromServices] CandidateItemViewModel.Factory candidateItemViewModelFactory)
        {
            var candidates = QueryProcessor.Find(new IsProjectCandidateSpec(ProjectId));
            
            var candidateViewModels = candidates.Select(candidateItemViewModelFactory.Create);
            var model = new VotingViewModel
            {
                Candidates = candidateViewModels
            };

            return View(nameof(Voting), model);
        }

        [HttpGet("add")]
        [Authorize(Policies.CanAddProjectCandidate)]
        public IActionResult AddCandidate()
        {
            var model = new AddCandidateViewModel();
            return View(nameof(AddCandidate), model);
        }

        [HttpPost("add")]
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

        [HttpGet("{candidateId}/delete")]
        public async Task<IActionResult> RemoveCandidate()
        {
            var canRemoveCandidate = (await AuthorizationService.AuthorizeAsync(User, CurrentCandidate, Policies.CanRemoveCandidate)).Succeeded;
            if (!canRemoveCandidate)
            {
                return Forbid();
            }

            return View(new RemoveCandidateViewModel());
        }

        [HttpPost("{candidateId}/delete")]
        public async Task<IActionResult> RemoveCandidate(RemoveCandidateViewModel model)
        {
            var canRemoveCandidate = (await AuthorizationService.AuthorizeAsync(User, CurrentCandidate, Policies.CanRemoveCandidate)).Succeeded;
            if (!canRemoveCandidate)
            {
                return Forbid();
            }

            ExecuteUserProjectCommand(new RemoveCandidateCommand
            {
                CandidateId = CandidateId
            }, Notice.Success(SuccessTexts.CandidateRemoved));

            return RedirectToAction(nameof(Voting), "Candidates");
        }

        [HttpGet("{candidateId}")]
        public virtual async Task<IActionResult> Candidate([FromServices]CandidateViewModel.Factory candidateViewModelFactory)
        {
            var candidateDiscussion = QueryProcessor.FindById<Discussion>(CurrentCandidate.Id);
            var model = await candidateViewModelFactory.Create(CurrentCandidate, candidateDiscussion, User);

            return View(nameof(Candidate), model);
        }

        [HttpGet("{candidateId}/edit")]
        public async Task<IActionResult> EditCandidate(Guid candidateId)
        {
            var canEditProjectCandidate = (await AuthorizationService.AuthorizeAsync(User, CurrentCandidate, Policies.CanEditProjectCandidate)).Succeeded;
            if (!canEditProjectCandidate)
            {
                return Forbid();
            }

            var model = AutoMapper.Map<EditCandidateViewModel>(CurrentCandidate);

            return View(nameof(EditCandidate), model);
        }

        [HttpPost("{candidateId}/edit")]
        public async Task<IActionResult> EditCandidate(EditCandidateViewModel model)
        {
            var canEditProjectCandidate = (await AuthorizationService.AuthorizeAsync(User, CurrentCandidate, Policies.CanEditProjectCandidate)).Succeeded;
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

            return RedirectToAction(nameof(Candidate));
        }

        [HttpGet("{candidateId}/change-image")]
        public async Task<IActionResult> EditCandidateImage()
        {
            var canChangeProjectCandidateImage = (await AuthorizationService.AuthorizeAsync(User, CurrentCandidate, Policies.CanChangeProjectCandidateImage)).Succeeded;
            if (!canChangeProjectCandidateImage)
            {
                return Forbid();
            }

            var model = AutoMapper.Map<EditCandidateImageViewModel>(CurrentCandidate);

            return View(nameof(EditCandidateImage), model);
        }

        [HttpPost("{candidateId}/change-image")]
        public async Task<IActionResult> EditCandidateImage(EditCandidateImageViewModel model)
        {
            var canChangeProjectCandidateImage = (await AuthorizationService.AuthorizeAsync(User, CurrentCandidate, Policies.CanChangeProjectCandidateImage)).Succeeded;
            if (!canChangeProjectCandidateImage)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return await EditCandidateImage();
            }

            var imageLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, model.Id);
            var imageStream = model.Image.OpenReadStream();
            await _imageStorageProvider.SaveImageAsync(imageStream, imageLocation, SizeOptions.AllDefaultSizes);

            ExecuteUserProjectCommand(new EditCandidateImageCommand
            {
                Id = CandidateId,
                ImageLocation = imageLocation
            }, Notice.Success(SuccessTexts.CandidateImageUploaded));

            if (!ModelState.IsValid)
            {
                return await EditCandidateImage();
            }

            return RedirectToAction(nameof(Candidate));
        }

        [HttpGet("{candidateId}/remove-image")]
        public async Task<IActionResult> RemoveCandidateImage()
        {
            var canChangeProjectCandidateImage = (await AuthorizationService.AuthorizeAsync(User, CurrentCandidate, Policies.CanRemoveProjectCandidateImage)).Succeeded;
            if (!canChangeProjectCandidateImage)
            {
                return Forbid();
            }
            var model = AutoMapper.Map<RemoveCandidateImageViewModel>(CurrentCandidate);

            return View(nameof(RemoveCandidateImage), model);
        }

        [HttpPost("{candidateId}/remove-image")]
        public async Task<IActionResult> RemoveCandidateImage(RemoveCandidateImageViewModel model)
        {
            var canChangeProjectCandidateImage = (await AuthorizationService.AuthorizeAsync(User, CurrentCandidate, Policies.CanRemoveProjectCandidateImage)).Succeeded;
            if (!canChangeProjectCandidateImage)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return await RemoveCandidateImage();
            }

            await _imageStorageProvider.DeleteImagesAsync(BlobLocations.GetProjectCandidateBlobLocation(ProjectId, CandidateId));

            ExecuteUserProjectCommand(new DeleteCandidateImageCommand
            {
                CandidateId = CandidateId,
                Actor = UserInfo
            }, Notice.Success(SuccessTexts.CandidateImageDeleted));

            if (!ModelState.IsValid)
            {
                return await RemoveCandidateImage();
            }

            return RedirectToAction(nameof(Candidate));
        }

        [HttpPost("{candidateId}/add-comment")]
        public async Task<IActionResult> AddComment(AddCommentModel model, [FromServices] CandidateViewModel.Factory candidateViewModelFactory)
        {
            if(!ModelState.IsValid)
            {
                return await Candidate(candidateViewModelFactory);
            }

            var project = QueryProcessor.FindById<Project>(this.ProjectId);
            var roleKeys = project.Members
                .Where(m => m.UserId == this.UserId)
                .Select(x => x.RoleKey)
                .ToArray();

            ExecuteUserProjectCommand(new AddCommentCommand
            {
                CommentText = model.CommentText,
                DiscussionId = CurrentCandidate.Id,
                AdditionalCommentData = new Dictionary<string, object> { { "RoleKeys", roleKeys } }.ToImmutableDictionary()
            }, Notice.Success(SuccessTexts.CommentAdded));
            
            if (!ModelState.IsValid)
            {
                return await Candidate(candidateViewModelFactory);
            }

            return RedirectToAction(nameof(Candidate));
        }

        [HttpPost("{candidateId}/edit-comment")]
        public async Task<IActionResult> EditComment(EditCommentModel model, [FromServices] CandidateViewModel.Factory candidateViewModelFactory)
        {
            if (!ModelState.IsValid)
            {
                return await Candidate(candidateViewModelFactory);
            }

            var candidateDiscussion = QueryProcessor.FindById<Discussion>(CurrentCandidate.Id);
            var comment = candidateDiscussion.Comments.Single(x => x.Id == model.CommentId);

            var canEditComment = (await AuthorizationService.AuthorizeAsync(User, comment, Policies.CanEditCandidateComment)).Succeeded;
            if (!canEditComment)
            {
                return Forbid();
            }

            var project = QueryProcessor.FindById<Project>(this.ProjectId);
            var roleKeys = project.Members
                .Where(m => m.UserId == this.UserId)
                .Select(x => x.RoleKey)
                .ToArray();

            ExecuteUserProjectCommand(new EditCommentCommand
            {
                DiscussionId = CandidateId,
                CommentText = model.CommentText,
                CommentId = model.CommentId,
                AdditionalCommentData = new Dictionary<string, object> { { "RoleKeys", roleKeys } }.ToImmutableDictionary()
            }, Notice.Success(SuccessTexts.CommentEdited));

            if (!ModelState.IsValid)
            {
                return await Candidate(candidateViewModelFactory);
            }

            return RedirectToAction(nameof(Candidate));
        }

        [HttpPost("{candidateId}/delete-comment")]
        public async Task<IActionResult> RemoveComment(Guid commentId, [FromServices] CandidateViewModel.Factory candidateViewModelFactory)
        {
            if (!ModelState.IsValid)
            {
                return await Candidate(candidateViewModelFactory);
            }

            var candidateDiscussion = QueryProcessor.FindById<Discussion>(CurrentCandidate.Id);
            var comment = candidateDiscussion.Comments.Single(x => x.Id == commentId);
            var canEditComment = (await AuthorizationService.AuthorizeAsync(User, comment, Policies.CanEditCandidateComment)).Succeeded;
            if (!canEditComment)
            {
                return Forbid();
            }

            ExecuteUserProjectCommand(new DeleteCommentCommand
            {
                DiscussionId = CurrentCandidate.Id,
                CommentId = commentId
            }, Notice.Success(SuccessTexts.CommentDeleted));

            if (!ModelState.IsValid)
            {
                return await Candidate(candidateViewModelFactory);
            }

            return RedirectToAction(nameof(Candidate));
        }
         
        [HttpPost("{candidateId}/vote")]
        public async Task<IActionResult> AddVote(AddVoteViewModel model, [FromServices] CandidateViewModel.Factory candidateViewModelFactory)
        {
            if (!ModelState.IsValid)
            {
                return await Candidate(candidateViewModelFactory);
            }

            var canVoteForCandidate = await AuthorizationService.IsAuthorizedAsync(User, CurrentCandidate, Policies.CanVoteCandidate);
            if (!canVoteForCandidate)
            {
                return Forbid();
            }

            ExecuteUserProjectCommand(new AddCandidateVoteCommand
            {
                CandidateId = CandidateId,
                Functionality = model.Functionality,
                Safety = model.Safety,
                Performance = model.Performace,
                Usability = model.Usability
            }, Notice.Success(SuccessTexts.CandidateVoteAdded));

            if (!ModelState.IsValid)
            {
                return await Candidate(candidateViewModelFactory);
            }

            return RedirectToAction(nameof(Candidate));
        }
    }
}
