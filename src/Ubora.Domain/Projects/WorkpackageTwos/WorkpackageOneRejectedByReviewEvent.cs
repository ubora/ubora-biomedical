using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    public class WorkpackageOneRejectedByReviewEvent : UboraEvent
    {
        public WorkpackageOneRejectedByReviewEvent(UserInfo initiatedBy, Guid workpackageOneId, string concludingComment) : base(initiatedBy)
        {
            WorkpackageOneId = workpackageOneId;
            ConcludingComment = concludingComment;
        }

        public Guid WorkpackageOneId { get; private set; }
        public string ConcludingComment { get; private set; }

        public override string GetDescription() => "Rejected workpackage by review.";
    }
}