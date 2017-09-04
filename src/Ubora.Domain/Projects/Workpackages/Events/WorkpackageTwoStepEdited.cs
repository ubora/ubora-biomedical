using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageTwoStepEdited : UboraEvent
    {
        public WorkpackageTwoStepEdited(UserInfo initiatedBy, string stepId, string title, string newValue)
            : base(initiatedBy)
        {
            StepId = stepId;
            Title = title;
            NewValue = newValue;
        }

        public string StepId { get; private set; }
        public string Title { get; private set; }
        public string NewValue { get; private set; }

        public override string GetDescription() => $"edited workpacage 2 \"{Title}\"";
    }
}
