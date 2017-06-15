using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Specifications
{
    public class CanBeSubmittedForReview<TWorkpackage> 
        : WrappedSpecification<TWorkpackage> where TWorkpackage : Workpackage<TWorkpackage>
    {
        public override Specification<TWorkpackage> ToSpecification()
        {
            var isAlreadyAccepted = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.Accepted);
            var isAlreadyInReview = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.InReview);

            return !(isAlreadyAccepted || isAlreadyInReview);
        }
    }
}