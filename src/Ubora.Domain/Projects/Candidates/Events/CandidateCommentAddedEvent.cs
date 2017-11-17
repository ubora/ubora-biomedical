using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Candidates.Events
{
    public class CandidateCommentAddedEvent : ProjectEvent
    {
        public CandidateCommentAddedEvent(UserInfo initiatedBy, Guid projectId, string commentText, Guid commentId, DateTime commentedAt) : base(initiatedBy, projectId)
        {
            CommentText = commentText;
            CommentId = commentId;
            CommentedAt = commentedAt;
        }

        public Guid CommentId { get; set; }
        public string CommentText { get; private set; }
        public DateTime CommentedAt { get; private set; }

        public override string GetDescription()
        {
            return "added new comment for candidate.";
        }
    }
}
