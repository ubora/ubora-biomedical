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
        public virtual WorkpackageReviewStatus Status { get; private set; }
        public string ConcludingComment { get; private set; }
        public DateTimeOffset SubmittedAt { get; private set; }
        public DateTimeOffset? ConcludedAt { get; private set; }

        public static WorkpackageReview Create(Guid id, DateTimeOffset submittedAt)
        {
            if (id == default(Guid))
            {
                throw new ArgumentException(id.ToString(), nameof(id));
            }
            if (submittedAt == default(DateTimeOffset))
            {
                throw new ArgumentException(submittedAt.ToString(), nameof(submittedAt));
            }

            return new WorkpackageReview
            {
                Id = id,
                SubmittedAt = submittedAt
            };
        }

        public WorkpackageReview ToAccepted(string concludingComment, DateTimeOffset concludedAt)
        {
            if (concludedAt == default(DateTimeOffset))
            {
                throw new ArgumentException(concludedAt.ToString(), nameof(concludedAt));
            }

            return new WorkpackageReview
            {
                Id = this.Id,
                SubmittedAt = this.SubmittedAt,
                Status = WorkpackageReviewStatus.Accepted,
                ConcludingComment = concludingComment,
                ConcludedAt = concludedAt
            };
        }

        public WorkpackageReview ToRejected(string concludingComment, DateTimeOffset concludedAt)
        {
            if (concludedAt == default(DateTimeOffset))
            {
                throw new ArgumentException(concludedAt.ToString(), nameof(concludedAt));
            }

            return new WorkpackageReview
            {
                Id = this.Id,
                SubmittedAt = this.SubmittedAt,
                Status = WorkpackageReviewStatus.Rejected,
                ConcludingComment = concludingComment,
                ConcludedAt = concludedAt
            };
        }

        public WorkpackageReview ToWorkpackageReopened()
        {
            if (Status != WorkpackageReviewStatus.Accepted)
            {
                throw new InvalidOperationException();
            }

            return new WorkpackageReview
            {
                Id = this.Id,
                SubmittedAt = this.SubmittedAt,
                Status = WorkpackageReviewStatus.AcceptedButWorkpackageReopened,
                ConcludingComment = this.ConcludingComment,
                ConcludedAt = this.ConcludedAt
            };
        }
    }
}