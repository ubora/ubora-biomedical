﻿using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageTwoReviewAcceptedEvent : UboraEvent
    {
        public WorkpackageTwoReviewAcceptedEvent(UserInfo initiatedBy, Guid projectId, string concludingComment, DateTimeOffset acceptedAt)
            : base(initiatedBy)
        {
            ProjectId = projectId;
            ConcludingComment = concludingComment;
            AcceptedAt = acceptedAt;
        }

        public Guid ProjectId { get; private set; }
        public string ConcludingComment { get; private set; }
        public DateTimeOffset AcceptedAt { get; private set; }

        public override string GetDescription() => $"accepted workpackage 2 by {StringTokens.WorkpackageTwoReview()}.";
    }
}
