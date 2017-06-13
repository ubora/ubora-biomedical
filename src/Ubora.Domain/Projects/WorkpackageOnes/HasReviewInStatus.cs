using System;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.WorkpackageOnes
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
                Enumerable.Any<WorkpackageReview>(workpackage.Reviews, review => review.Status == this.Status);
        }
    }
}