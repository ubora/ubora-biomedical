using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Specifications
{
    public class IsWorkpackageOneLocked : WrappedSpecification<WorkpackageOne>
    {
        internal override Specification<WorkpackageOne> WrapSpecifications()
        {
            var isInReview = new HasReviewInStatus<WorkpackageOne>(WorkpackageReviewStatus.InProcess);
            var isAcceptedByReview = new HasReviewInStatus<WorkpackageOne>(WorkpackageReviewStatus.Accepted);

            return isAcceptedByReview || isInReview;
        }
    }
}