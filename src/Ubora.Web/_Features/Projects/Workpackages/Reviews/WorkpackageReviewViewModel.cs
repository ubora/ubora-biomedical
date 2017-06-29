using System;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageReviewViewModel
    {
        public WorkpackageReviewStatus Status { get; set; }
        public string ConcludingComment { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
        public DateTimeOffset? ConcludedAt { get; set; }
    }
}