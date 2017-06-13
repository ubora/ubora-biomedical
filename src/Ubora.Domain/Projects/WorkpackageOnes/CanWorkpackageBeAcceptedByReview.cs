using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public class CanWorkpackageBeAcceptedByReview<TWorkpackage> 
        : WrappedSpecification<TWorkpackage> where TWorkpackage : Workpackage<TWorkpackage>
    {
        public override Specification<TWorkpackage> ToSpecification()
        {
            var isInReview = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.InReview);
            var isAlreadyAccepted = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.Accepted);

            return isInReview && !isAlreadyAccepted;
        }
    }
}