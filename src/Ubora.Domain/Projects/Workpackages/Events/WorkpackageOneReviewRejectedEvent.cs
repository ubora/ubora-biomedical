using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneReviewRejectedEvent : UboraEvent
    {
        public WorkpackageOneReviewRejectedEvent(UserInfo initiatedBy, Guid projectId, string concludingComment, DateTimeOffset rejectedAt)
            : base(initiatedBy)
        {
            ProjectId = projectId;
            ConcludingComment = concludingComment;
            RejectedAt = rejectedAt;
        }

        public Guid ProjectId { get; private set; }
        public string ConcludingComment { get; private set; }
        public DateTimeOffset RejectedAt { get; private set; }

        public override string GetDescription() => $"rejected workpackage 1 by {StringTokens.Review()}.";
    }
}