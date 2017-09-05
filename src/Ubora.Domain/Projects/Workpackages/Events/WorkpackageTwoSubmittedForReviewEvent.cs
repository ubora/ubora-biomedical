using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageTwoSubmittedForReviewEvent : UboraEvent
    {
        public WorkpackageTwoSubmittedForReviewEvent(UserInfo initiatedBy, Guid projectId, Guid reviewId, DateTimeOffset submittedAt) 
            : base(initiatedBy)
        {
            ProjectId = projectId;
            ReviewId = reviewId;
            SubmittedAt = submittedAt;
        }

        public Guid ProjectId { get; private set; }
        public Guid ReviewId { get; private set; }
        public DateTimeOffset SubmittedAt { get; private set; }

        public override string GetDescription() => $"submitted workpackage 2 for {StringTokens.WorkpackageTwoReview()}.";
    }
}