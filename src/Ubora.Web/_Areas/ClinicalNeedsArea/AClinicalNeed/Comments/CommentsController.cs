using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Discussions;
using Ubora.Domain.Discussions.Commands;
using Ubora.Web.Infrastructure;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview;
using Ubora.Web._Components.Discussions.Models;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Comments
{
    public class CommentsController : AClinicalNeedController
    {
        public Discussion Discussion { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Discussion = QueryProcessor.FindById<Discussion>(ClinicalNeed.Id);
        }

        [HttpPost("add-comment")]
        [SaveTempDataModelState]
        public async Task<IActionResult> AddComment(AddCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return await RedirectToOverview();
            }

            ExecuteUserCommand(new AddCommentCommand
            {
                CommentText = model.CommentText,
                DiscussionId = Discussion.Id,
                AdditionalCommentData = new Dictionary<string, object>().ToImmutableDictionary()
            }, Notice.Success("TODO"));

            if (!ModelState.IsValid)
            {
                return await RedirectToOverview();
            }

            return await RedirectToOverview();
        }

        [HttpPost("edit-comment")]
        [SaveTempDataModelState]
        public async Task<IActionResult> EditComment(EditCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return await RedirectToOverview();
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
            }, Notice.Success("TODO"));

            if (!ModelState.IsValid)
            {
                return await RedirectToOverview();
            }

            return await RedirectToOverview();
        }

        [HttpPost("delete-comment")]
        [SaveTempDataModelState]
        public async Task<IActionResult> RemoveComment(Guid commentId)
        {
            if (!ModelState.IsValid)
            {
                return await RedirectToOverview();
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
            }, Notice.Success("TODO"));

            if (!ModelState.IsValid)
            {
                return await RedirectToOverview();
            }

            return await RedirectToOverview();
        }

        private Task<RedirectToActionResult> RedirectToOverview()
        {
            return Task.FromResult(RedirectToAction(nameof(OverviewController.Overview), nameof(OverviewController).RemoveSuffix()));
        }
    }
}
