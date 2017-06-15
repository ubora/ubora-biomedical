using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneSubmittedForReviewEvent : UboraEvent
    {
        public WorkpackageOneSubmittedForReviewEvent(UserInfo initiatedBy, Guid workpackageOneId) : base(initiatedBy)
        {
            WorkpackageOneId = workpackageOneId;
        }

        public Guid WorkpackageOneId { get; set; }

        public override string GetDescription() => "Submitted for review.";
    }
}