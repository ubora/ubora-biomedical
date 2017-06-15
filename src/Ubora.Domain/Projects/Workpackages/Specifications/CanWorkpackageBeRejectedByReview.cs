using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Specifications
{
    public class CanWorkpackageBeRejectedByReview<TWorkpackage> 
        : WrappedSpecification<TWorkpackage> where TWorkpackage : Workpackage<TWorkpackage>
    {
        public override Specification<TWorkpackage> ToSpecification()
        {
            var isInReview = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.InReview);

            return isInReview;
        }
    }
}