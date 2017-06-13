using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    public class WorkpackageOneAcceptedByReviewEvent : UboraEvent
    {
        public WorkpackageOneAcceptedByReviewEvent(UserInfo initiatedBy, Guid workpackageOneId, Guid reviewId) : base(initiatedBy)
        {
            WorkpackageOneId = workpackageOneId;
            ReviewId = reviewId;
        }

        public Guid WorkpackageOneId { get; private set; }
        public Guid ReviewId { get; private set; }

        public override string GetDescription() => "Accepted workpackage.";
    }
}