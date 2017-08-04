using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Specifications
{
    public class CanWorkpackageBeAcceptedByReview<TWorkpackage> 
        : WrappedSpecification<TWorkpackage> where TWorkpackage : Workpackage<TWorkpackage>
    {
        internal override Specification<TWorkpackage> WrapSpecifications()
        {
            var isInReview = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.InProcess);
            var isAlreadyAccepted = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.Accepted);

            return isInReview && !isAlreadyAccepted;
        }
    }
}