using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Discussions.Events
{
    public class CommentDeletedEvent : UboraEvent
    {
        public Guid CommentId { get; }
        public Guid? ProjectId { get; }

        public CommentDeletedEvent(UserInfo initiatedBy, Guid commentId, Guid? projectId) 
            : base(initiatedBy)
        {
            CommentId = commentId;
            ProjectId = projectId;
        }

        public override string GetDescription()
        {
            return "deleted comment.";
        }
    }
}