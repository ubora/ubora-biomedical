using System;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Specifications
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