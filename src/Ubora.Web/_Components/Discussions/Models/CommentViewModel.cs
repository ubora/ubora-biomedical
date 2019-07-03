using System;

namespace Ubora.Web._Components.Discussions.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid CommentatorId { get; set; }
        public string CommentatorName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CommentText { get; set; }
        public DateTimeOffset CommentedAt { get; set; }
        public DateTimeOffset? LastEditedAt { get; set; }
        public bool CanBeEdited { get; set; }

        public bool IsLeader { get; set; } // TODO
        public bool IsMentor { get; set; } // TODO

        public DateTimeOffset CommentEditedAt => LastEditedAt ?? CommentedAt;

        public bool IsEdited => LastEditedAt.HasValue;
    }
}