using System;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageOneReview
{
    public class WorkpackageReviewViewModel
    {
        public WorkpackageReviewStatus Status { get; set; }
        public string ConcludingComment { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
        public DateTimeOffset? ConcludedAt { get; set; }
    }
}