using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageStepEditedEvent : UboraEvent
    {
        public Guid StepId { get; set; }
        public string Title { get; set; }
        public string NewValue { get; set; }

        public WorkpackageStepEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            return $"Edited \"{Title}\"";
        }
    }
}