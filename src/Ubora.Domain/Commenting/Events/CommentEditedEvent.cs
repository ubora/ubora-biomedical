using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Commenting.Events
{
    public class CommentEditedEvent : UboraEvent
    {
        public Guid CommentId { get; }
        public string CommentText { get; }
        public Guid? ProjectId { get; }
        public Dictionary<string, object> Additional { get; }

        public CommentEditedEvent(UserInfo initiatedBy, Guid commentId, string commentText, Guid? projectId, Dictionary<string, object> additional) 
            : base(initiatedBy)
        {
            CommentId = commentId;
            CommentText = commentText;
            ProjectId = projectId;
            Additional = additional;
        }

        public override string GetDescription()
        {
            return "edited comment.";
        }
    }
}