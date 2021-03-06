using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Specifications
{
    public class CanSubmitWorkpackageReview<TWorkpackage> 
        : WrappedSpecification<TWorkpackage> where TWorkpackage : Workpackage<TWorkpackage>
    {
        internal override Specification<TWorkpackage> WrapSpecifications()
        {
            var isAlreadyAccepted = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.Accepted);
            var isAlreadyInReview = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.InProcess);

            return !(isAlreadyAccepted || isAlreadyInReview);
        }
    }
}