using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Discussions;
using Ubora.Web.Infrastructure;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Comments;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview.Models;
using Ubora.Web._Components.Discussions.Models;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview
{
    public class OverviewController : AClinicalNeedController
    {
        [RestoreModelStateFromTempData]
        public async Task<IActionResult> Overview()
        {
            var discussion = QueryProcessor.FindById<Discussion>(ClinicalNeed.Id);

            var model = new OverviewViewModel
            {
                Title = ClinicalNeed.Title,
                Description = ClinicalNeed.Description,
                AreaOfUsageTag = ClinicalNeed.AreaOfUsageTag,
                ClinicalNeedTag = ClinicalNeed.ClinicalNeedTag,
                PotentialTechnologyTag = ClinicalNeed.PotentialTechnologyTag,
                Keywords = ClinicalNeed.Keywords,
                IndicatedAt = ClinicalNeed.IndicatedAt,
                IndicatorUserId = ClinicalNeed.IndicatorUserId,
                Discussion = new DiscussionViewModel
                {
                    AddCommentActionPath = Url.Action(nameof(CommentsController.AddComment), "Comments"),
                    EditCommentActionPath = Url.Action(nameof(CommentsController.EditComment), "Comments"),
                    DeleteCommentActionPath = Url.Action(nameof(CommentsController.RemoveComment), "Comments"),
                    Comments = discussion.Comments.Select(async c => new CommentViewModel
                    {
                        Id = c.Id,
                        CanBeEdited = (await AuthorizationService.AuthorizeAsync(User, c, Policies.CanEditClinicalNeedComment)).Succeeded,
                        CommentText = c.Text,
                        CommentatorId = c.UserId,
                        CommentatorName = "test",
                        CommentedAt = c.CommentedAt,
                        LastEditedAt = c.LastEditedAt,
                        IsLeader = false,
                        IsMentor = false,
                        ProfilePictureUrl = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png"
                    }).Select(t => t.Result).ToList()
                }
            };

            return View(model);
        }
    }
}