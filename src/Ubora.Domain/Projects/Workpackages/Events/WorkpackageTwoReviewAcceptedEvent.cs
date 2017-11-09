using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageTwoReviewAcceptedEvent : ProjectEvent
    {
        public WorkpackageTwoReviewAcceptedEvent(UserInfo initiatedBy, Guid projectId, string concludingComment, DateTimeOffset acceptedAt) : base(initiatedBy, projectId)
        {
            ConcludingComment = concludingComment;
            AcceptedAt = acceptedAt;
        }

        public string ConcludingComment { get; private set; }
        public DateTimeOffset AcceptedAt { get; private set; }

        public override string GetDescription() => $"accepted workpackage 2 by {StringTokens.WorkpackageTwoReview()}.";
    }
}
