using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public class WorkpackageOneStepEditedEvent : UboraEvent
    {
        public Guid StepId { get; set; }
        public string Title { get; set; }
        public string NewValue { get; set; }

        public WorkpackageOneStepEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            return $"Edited \"{Title}\"";
        }
    }
}