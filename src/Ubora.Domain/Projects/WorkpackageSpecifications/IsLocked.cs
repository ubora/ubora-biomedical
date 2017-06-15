using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects.WorkpackageSpecifications
{
    public class IsLocked : WrappedSpecification<WorkpackageOne>
    {
        public override Specification<WorkpackageOne> ToSpecification()
        {
            var isInReview = new HasReviewInStatus<WorkpackageOne>(WorkpackageReviewStatus.InReview);
            var isAcceptedByReview = new HasReviewInStatus<WorkpackageOne>(WorkpackageReviewStatus.Accepted);

            return isAcceptedByReview || isInReview;
        }
    }
}