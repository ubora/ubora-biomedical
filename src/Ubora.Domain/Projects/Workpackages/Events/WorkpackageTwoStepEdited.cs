using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageTwoStepEdited : ProjectEvent
    {
        public WorkpackageTwoStepEdited(UserInfo initiatedBy, Guid projectId, string stepId, string title, string newValue) : base(initiatedBy, projectId)
        {
            StepId = stepId;
            Title = title;
            NewValue = newValue;
        }

        public string StepId { get; private set; }
        public string Title { get; private set; }
        public string NewValue { get; private set; }

        public override string GetDescription() => $"edited workpackage 2 \"{Title}\"";
    }
}
