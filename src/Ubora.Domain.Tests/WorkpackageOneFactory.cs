using System.Collections.Generic;
using Moq;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Tests
{
    public static class WorkpackageOneFactory
    {
        public static WorkpackageOne Create(params WorkpackageReviewStatus[] reviewsInStatus)
        {
            var reviews = new HashSet<WorkpackageReview>();
            foreach (var reviewStatus in reviewsInStatus)
            {
                var review = Mock.Of<WorkpackageReview>(x => x.Status == reviewStatus);
                reviews.Add(review);
            }

            var workpackage = Mock.Of<WorkpackageOne>(x => x.Reviews == reviews);
            return workpackage;
        }
    }
}