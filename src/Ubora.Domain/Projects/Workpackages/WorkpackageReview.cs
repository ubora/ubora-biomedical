using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageReview
    {
        [JsonConstructor]
        protected WorkpackageReview()
        {
        }

        public Guid Id { get; private set; }
        public WorkpackageReviewStatus Status { get; private set; }
        public string ConcludingComment { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? ConcludedAt { get; private set; }

        public static WorkpackageReview Create()
        {
            return new WorkpackageReview
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.Now
            };
        }

        public WorkpackageReview ToAccepted(string concludingComment)
        {
            return new WorkpackageReview
            {
                Id = this.Id,
                CreatedAt = this.CreatedAt,
                Status = WorkpackageReviewStatus.Accepted,
                ConcludingComment = concludingComment,
                ConcludedAt = DateTimeOffset.Now
            };
        }

        public WorkpackageReview ToRejected(string concludingComment)
        {
            return new WorkpackageReview
            {
                Id = this.Id,
                CreatedAt = this.CreatedAt,
                Status = WorkpackageReviewStatus.Rejected,
                ConcludingComment = concludingComment,
                ConcludedAt = DateTimeOffset.Now
            };
        }
    }
}