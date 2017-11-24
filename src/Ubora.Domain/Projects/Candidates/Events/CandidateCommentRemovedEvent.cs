using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Candidates.Events
{
    public class CandidateCommentRemovedEvent : ProjectEvent
    {
        public CandidateCommentRemovedEvent(UserInfo initiatedBy, Guid projectId, Guid commentId) : base(initiatedBy, projectId)
        {
            CommentId = commentId;
        }

        public Guid CommentId { get; private set; }

        public override string GetDescription()
        {
            return "deleted candidate comment.";
        }
    }
}
