using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    public class WorkpackageOneAcceptedByReviewEvent : UboraEvent
    {
        public WorkpackageOneAcceptedByReviewEvent(UserInfo initiatedBy, Guid projectId, Guid reviewId) : base(initiatedBy)
        {
            ProjectId = projectId;
            ReviewId = reviewId;
        }

        public Guid ProjectId { get; private set; }
        public Guid ReviewId { get; private set; }

        public override string GetDescription() => "Accepted workpackage.";
    }
}