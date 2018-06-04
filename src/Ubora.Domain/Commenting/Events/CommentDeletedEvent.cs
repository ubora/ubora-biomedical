using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Commenting.Events
{
    public class CommentDeletedEvent : UboraEvent
    {
        public Guid CommentId { get; }
        public Guid? ProjectId { get; }
        public Dictionary<string, object> Additional { get; }

        public CommentDeletedEvent(UserInfo initiatedBy, Guid commentId, Guid? projectId, Dictionary<string, object> additional) 
            : base(initiatedBy)
        {
            CommentId = commentId;
            ProjectId = projectId;
            Additional = additional;
        }

        public override string GetDescription()
        {
            return "deleted comment.";
        }
    }
}
