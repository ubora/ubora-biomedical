using System;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public class WorkpackageReview
    {
        public WorkpackageReview(Guid id, WorkpackageReviewStatus status)
        {
            Id = id;
            Status = status;
        }

        public Guid Id { get; }
        public WorkpackageReviewStatus Status { get; }

        public WorkpackageReview ToStatus(WorkpackageReviewStatus status)
        {
            return new WorkpackageReview(Id, status);
        }
    }
}