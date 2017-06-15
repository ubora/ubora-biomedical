using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneAcceptedByReviewEvent : UboraEvent
    {
        public WorkpackageOneAcceptedByReviewEvent(UserInfo initiatedBy, Guid projectId, string concludingComment) 
            : base(initiatedBy)
        {
            ProjectId = projectId;
            ConcludingComment = concludingComment;
        }

        public Guid ProjectId { get; private set; }
        public string ConcludingComment { get; private set; }

        public override string GetDescription() => "Accepted workpackage.";
    }
}