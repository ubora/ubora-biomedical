using System;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects.WorkpackageSpecifications
{
    public class CanWorkpackageOneStepBeEdited : Specification<WorkpackageOne>
    {
        public CanWorkpackageOneStepBeEdited(Guid stepId)
        {
            StepId = stepId;
        }

        public Guid StepId { get; }

        internal override Expression<Func<WorkpackageOne, bool>> ToExpression()
        {
            return wp => !wp.IsLocked && wp.Steps.Any(s => s.Id == StepId);
        }
    }
}