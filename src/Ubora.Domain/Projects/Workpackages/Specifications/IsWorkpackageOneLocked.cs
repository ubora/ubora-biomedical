using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Specifications
{
    public class IsWorkpackageOneLocked : WrappedSpecification<WorkpackageOne>
    {
        public override Specification<WorkpackageOne> ToSpecification()
        {
            var isInReview = new HasReviewInStatus<WorkpackageOne>(WorkpackageReviewStatus.InProcess);
            var isAcceptedByReview = new HasReviewInStatus<WorkpackageOne>(WorkpackageReviewStatus.Accepted);

            return isAcceptedByReview || isInReview;
        }
    }
}