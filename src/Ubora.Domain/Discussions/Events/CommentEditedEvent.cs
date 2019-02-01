using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Discussions.Events
{
    public class CommentEditedEvent : ProjectEvent
    {
        public Guid CommentId { get; }
        public string CommentText { get; }
        public ImmutableDictionary<string, object> AdditionalCommentData { get; }

        public CommentEditedEvent(UserInfo initiatedBy, Guid commentId, string commentText, Guid projectId, ImmutableDictionary<string, object> additionalCommentData) 
            : base(initiatedBy, projectId)
        {
            CommentId = commentId;
            CommentText = commentText;
            AdditionalCommentData = additionalCommentData;
        }

        public override string GetDescription()
        {
            return "edited comment.";
        }
    }
}