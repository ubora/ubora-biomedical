using System;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects.WorkpackageSpecifications
{
    public class CanWorkpackageOneStepBeEdited : WrappedSpecification<WorkpackageOne>
    {
        public CanWorkpackageOneStepBeEdited(Guid stepId)
        {
            StepId = stepId;
        }

        public Guid StepId { get; }

        public override Specification<WorkpackageOne> ToSpecification()
        {
            var isLocked = new IsLocked();
            var hasStep = new HasStep<WorkpackageOne>(StepId);

            return !isLocked && hasStep;
        }
    }
}