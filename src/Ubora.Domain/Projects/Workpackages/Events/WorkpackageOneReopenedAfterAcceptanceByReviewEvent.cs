using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneReopenedAfterAcceptanceByReviewEvent : ProjectEvent
    {
        public WorkpackageOneReopenedAfterAcceptanceByReviewEvent(UserInfo initiatedBy, Guid projectId, Guid acceptedReviewId) 
            : base(initiatedBy, projectId)
        {
            AcceptedReviewId = acceptedReviewId;
        }

        public Guid AcceptedReviewId { get; private set; }

        public override string GetDescription() => "reopened WP1 for edits.";
    }
}
