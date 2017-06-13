using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects.WorkpackageSpecifications
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