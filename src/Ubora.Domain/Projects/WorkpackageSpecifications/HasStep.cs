using System;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects.WorkpackageSpecifications
{
    public class HasStep<TWorkpackage>
        : Specification<TWorkpackage> where TWorkpackage : Workpackage<TWorkpackage>
    {
        public Guid StepId { get; }

        public HasStep(Guid stepId)
        {
            StepId = stepId;
        }

        internal override Expression<Func<TWorkpackage, bool>> ToExpression()
        {
            return workpackage =>
                workpackage.Steps.Any<WorkpackageStep>(step => step.Id == StepId);
        }
    }
}