using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneSubmittedForReviewEvent : UboraEvent
    {
        public WorkpackageOneSubmittedForReviewEvent(UserInfo initiatedBy, Guid projectId, Guid reviewId, DateTimeOffset submittedAt) 
            : base(initiatedBy)
        {
            ProjectId = projectId;
            ReviewId = reviewId;
            SubmittedAt = submittedAt;
        }

        public Guid ProjectId { get; private set; }
        public Guid ReviewId { get; private set; }
        public DateTimeOffset SubmittedAt { get; private set; }

        public override string GetDescription() => $"submitted workpackage 1 for {StringTokens.Review()}.";
    }
}