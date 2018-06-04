using System;
using System.Collections.Generic;

namespace Ubora.Web._Components.Commenting
{
    public class DeleteCommentModel
    {
        public Guid CommentId { get; set; }
        public string CommentText { get; set; }
        public string DeleteCommentActionPath { get; set; }
    }

    public class EditCommentModel
    {
        public Guid CommentId { get; set; }
        public string CommentText { get; set; }
        public string EditCommentActionPath { get; set; }
    }

    public class AddCommentModel
    {
        public string CommentText { get; set; }
        public string AddCommentActionPath { get; set; }
    }

    public class DiscussionViewModel
    {
        public IReadOnlyCollection<CommentViewModel> Comments { get; set; }
        public string EditCommentActionPath { get; set; }
        public string DeleteCommentActionPath { get; set; }
        public string AddCommentActionPath { get; set; }
    }

    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid CommentatorId { get; set; }
        public string CommentatorName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentedAt { get; set; }
        public DateTime LastEditedAt { get; set; }
        public bool CanBeEdited { get; set; }
        public DateTime CommentEditedAt
        {
            get
            {
                return LastEditedAt > CommentedAt ? LastEditedAt : CommentedAt;
            }
        }

        public bool IsEdited
        {
            get
            {
                return LastEditedAt != DateTime.MinValue;
            }
        }
    }
}