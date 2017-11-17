using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Candidates.Events
{
    public class CandidateCommentAddedEvent : ProjectEvent
    {
        public CandidateCommentAddedEvent(UserInfo initiatedBy, Guid projectId, string commentText) : base(initiatedBy, projectId)
        {
            CommentText = commentText;
        }

        public string CommentText { get; private set; }

        public override string GetDescription()
        {
            return "added new comment for candidate.";
        }
    }
}
