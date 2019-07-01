using System;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageFive : Workpackage<WorkpackageFive>
    {
        private void Apply(WorkpackageFiveOpenedEvent e)
        {
            if (_steps.Any())
                throw new InvalidOperationException("Already opened.");

            ProjectId = e.ProjectId;
            Title = "Operation";

            _steps.Add(new WorkpackageStep("ProductionDocumentation", "Production documentation"));
        }

        private void Apply(WorkpackageFiveStepEditedEvent e) 
        {
            var step = _steps.Single(x => x.Id == e.StepId);
            step.Title = e.Title;
            step.ContentV2 = e.NewValue;
        }
    }
}
