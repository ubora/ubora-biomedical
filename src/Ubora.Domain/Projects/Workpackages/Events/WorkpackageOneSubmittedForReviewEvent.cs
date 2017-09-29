using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneSubmittedForReviewEvent : ProjectEvent
    {
        public WorkpackageOneSubmittedForReviewEvent(UserInfo initiatedBy, Guid projectId, Guid reviewId, DateTimeOffset submittedAt) : base(initiatedBy, projectId)
        {
            ReviewId = reviewId;
            SubmittedAt = submittedAt;
        }

        public Guid ReviewId { get; private set; }
        public DateTimeOffset SubmittedAt { get; private set; }

        public override string GetDescription() => $"submitted workpackage 1 for {StringTokens.WorkpackageOneReview()}.";
    }
}