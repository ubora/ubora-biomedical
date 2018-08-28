using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Discussions;
using Ubora.Domain.Discussions.Commands;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Comments.Models;
using Ubora.Web._Components.Discussions.Models;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Comments
{
    public class CommentsController : AClinicalNeedController
    {
        private readonly IQuerySession _querySession;
        private readonly ImageStorageProvider _imageStorageProvider;

        public Discussion Discussion { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Discussion = QueryProcessor.FindById<Discussion>(ClinicalNeed.Id);
        }

        public CommentsController(IQuerySession querySession, ImageStorageProvider imageStorageProvider)
        {
            _querySession = querySession;
            _imageStorageProvider = imageStorageProvider;
        }

        [HttpGet("comments")]
        public IActionResult Comments()
        {
            var commentatorIds = Discussion.Comments.Select(c => c.UserId).Distinct().ToArray();

            var commentators = _querySession.LoadMany<UserProfile>(commentatorIds)
                .Select(up => new
                {
                    up.UserId,
                    up.FullName,
                    ProfilePictureUrl = _imageStorageProvider.GetDefaultOrBlobUrl(up)
                })
                .ToDictionary(vm => vm.UserId, vm => vm);

            var model = new CommentsViewModel
            {
                Discussion = new DiscussionViewModel
                {
                    AddCommentActionPath = Url.Action(nameof(CommentsController.AddComment), "Comments"),
                    EditCommentActionPath = Url.Action(nameof(CommentsController.EditComment), "Comments"),
                    DeleteCommentActionPath = Url.Action(nameof(CommentsController.RemoveComment), "Comments"),
                    Comments = Discussion.Comments.Select(async c => new CommentViewModel
                    {
                        Id = c.Id,
                        CanBeEdited = (await AuthorizationService.AuthorizeAsync(User, c, Policies.CanEditClinicalNeedComment)).Succeeded,
                        CommentText = c.Text,
                        CommentatorId = c.UserId,
                        CommentatorName = commentators[c.UserId].FullName,
                        CommentedAt = c.CommentedAt,
                        LastEditedAt = c.LastEditedAt,
                        IsLeader = false,
                        IsMentor = false,
                        ProfilePictureUrl = commentators[c.UserId].ProfilePictureUrl
                    }).Select(t => t.Result).ToList()
                }
            };

            return View(nameof(Comments), model);
        }

        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment(AddCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return Comments();
            }

            ExecuteUserCommand(new AddCommentCommand
            {
                CommentText = model.CommentText,
                DiscussionId = Discussion.Id,
                AdditionalCommentData = new Dictionary<string, object>().ToImmutableDictionary()
            }, Notice.Success(SuccessTexts.CommentAdded));

            if (!ModelState.IsValid)
            {
                return Comments();
            }

            return RedirectToAction(nameof(Comments));
        }

        [HttpPost("edit-comment")]
        public async Task<IActionResult> EditComment(EditCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return Comments();
            }

            var comment = Discussion.Comments.Single(x => x.Id == model.CommentId);

            var isAuthorized = (await AuthorizationService.AuthorizeAsync(User, comment, Policies.CanEditClinicalNeedComment)).Succeeded;
            if (!isAuthorized)
            {
                return Forbid();
            }

            ExecuteUserCommand(new EditCommentCommand
            {
                DiscussionId = Discussion.Id,
                CommentText = model.CommentText,
                CommentId = model.CommentId,
                AdditionalCommentData = new Dictionary<string, object>().ToImmutableDictionary()
            }, Notice.Success(SuccessTexts.CommentEdited));

            if (!ModelState.IsValid)
            {
                return Comments();
            }

            return RedirectToAction(nameof(Comments));
        }

        [HttpPost("delete-comment")]
        public async Task<IActionResult> RemoveComment(Guid commentId)
        {
            if (!ModelState.IsValid)
            {
                return Comments();
            }

            var comment = Discussion.Comments.Single(x => x.Id == commentId);

            var isAuthorized = (await AuthorizationService.AuthorizeAsync(User, comment, Policies.CanEditClinicalNeedComment)).Succeeded;
            if (!isAuthorized)
            {
                return Forbid();
            }

            ExecuteUserCommand(new DeleteCommentCommand
            {
                DiscussionId = Discussion.Id,
                CommentId = commentId
            }, Notice.Success(SuccessTexts.CommentDeleted));

            if (!ModelState.IsValid)
            {
                return Comments();
            }

            return RedirectToAction(nameof(Comments));
        }
    }
}
