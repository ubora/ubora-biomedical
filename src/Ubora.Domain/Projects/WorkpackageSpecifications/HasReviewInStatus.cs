using System;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects.WorkpackageSpecifications
{
    public class HasReviewInStatus<TWorkpackage> 
        : Specification<TWorkpackage> where TWorkpackage : Workpackage<TWorkpackage>
    {
        public HasReviewInStatus(WorkpackageReviewStatus status)
        {
            Status = status;
        }

        public WorkpackageReviewStatus Status { get; set; }

        internal override Expression<Func<TWorkpackage, bool>> ToExpression()
        {
            return workpackage => 
                workpackage.Reviews.Any(review => review.Status == this.Status);
        }
    }
}