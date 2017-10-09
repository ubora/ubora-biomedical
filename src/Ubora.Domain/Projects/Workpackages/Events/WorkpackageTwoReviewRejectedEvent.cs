using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageTwoReviewRejectedEvent : ProjectEvent
    {
        public WorkpackageTwoReviewRejectedEvent(UserInfo initiatedBy, Guid projectId, string concludingComment, DateTimeOffset rejectedAt) : base(initiatedBy, projectId)
        {
            ConcludingComment = concludingComment;
            RejectedAt = rejectedAt;
        }

        public string ConcludingComment { get; private set; }
        public DateTimeOffset RejectedAt { get; private set; }

        public override string GetDescription() => $"rejected workpackage 2 by {StringTokens.WorkpackageTwoReview()}.";
    }
}
