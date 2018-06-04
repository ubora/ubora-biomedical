using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Commenting;
using Ubora.Domain.Commenting.Commands;
using Ubora.Web._Components.Commenting;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Comments
{
    public class CommentsController : ProjectController
    {
        private readonly IQuerySession _querySession;

        public CommentsController(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        public IActionResult Index()
        {
            var discussion = _querySession.Load<Discussion>(ProjectId);

            var model = new DiscussionViewModel
            {
                Comments = discussion?.Comments.Select(c => new CommentViewModel
                { 
                    Id = c.Id,
                    CommentedAt = c.CommentedAt,
                    CommentText = c.Text,
                    LastEditedAt = c.LastEditedAt,
                    CanBeEdited = true
                }).ToList() ?? new List<CommentViewModel>(),
                AddCommentActionPath = Url.Action(nameof(Add)),
                DeleteCommentActionPath = Url.Action(nameof(Delete)),
                EditCommentActionPath = Url.Action(nameof(Edit))
            };

            return View(nameof(Index), model);
        }

        [HttpPost]
        public IActionResult Delete(EditCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            ExecuteUserCommand(new DeleteCommentCommand
            {
                CommentId = model.CommentId,
                DiscussionId = ProjectId,
                ProjectId = ProjectId
            }, Notice.Success("todo"));

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Add(AddCommentModel model)
        {
            if (!ModelState.IsValid) 
            {
                return Index();
            }

            ExecuteUserCommand(new AddCommentCommand
            {
                CommentText = model.CommentText,
                DiscussionId = ProjectId,
                ProjectId = ProjectId
            }, Notice.Success("todo"));

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(EditCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            ExecuteUserCommand(new EditCommentCommand
            {
                CommentId = model.CommentId,
                CommentText = model.CommentText,
                DiscussionId = ProjectId,
                ProjectId = ProjectId
            }, Notice.Success("todo"));

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction("Index");
        }
    }
}
