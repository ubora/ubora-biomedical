using System;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Specifications
{
    public class HasReviewInStatus<TWorkpackage> 
        : Specification<TWorkpackage> where TWorkpackage : Workpackage<TWorkpackage>
    {
        public HasReviewInStatus(WorkpackageReviewStatus status)
        {
            Status = status;
        }

        public WorkpackageReviewStatus Status { get; }

        internal override Expression<Func<TWorkpackage, bool>> ToExpression()
        {
            return workpackage => 
                workpackage.Reviews.Any<WorkpackageReview>(review => review.Status == this.Status);
        }
    }
}