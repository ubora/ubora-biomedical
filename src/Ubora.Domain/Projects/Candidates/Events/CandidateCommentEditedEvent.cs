﻿using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Candidates.Events
{
    public class CandidateCommentEditedEvent : ProjectEvent
    {
        public CandidateCommentEditedEvent(UserInfo initiatedBy, Guid projectId, Guid commentId, string commentText, DateTime lastEditedAt, string[] roleKeys) 
            : base(initiatedBy, projectId)
        {
            CommentId = commentId;
            CommentText = commentText;
            LastEditedAt = lastEditedAt;
            RoleKeys = roleKeys;

        }

        public Guid CommentId { get; private set; }
        public string CommentText { get; private set; }
        public DateTime LastEditedAt { get; private set; }
        public string[] RoleKeys { get; private set; }

        public override string GetDescription()
        {
            return "edited candidate comment.";
        }
    }
}
