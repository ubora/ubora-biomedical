using System;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageSix : Workpackage<WorkpackageSix>
    {
        private void Apply(WorkpackageSixOpenedEvent e)
        {
            if (_steps.Any())
                throw new InvalidOperationException("Already opened.");

            ProjectId = e.ProjectId;
            Title = "Project closure";

            _steps.Add(new WorkpackageStep("InfoForGeneralPublic", "Info for general public"));
            _steps.Add(new WorkpackageStep("RealLifeUseOrSimulation", "Real life use or simulation"));
            _steps.Add(new WorkpackageStep("PresentationForPress", "Presentation for press"));
        }

        private void Apply(WorkpackageSixStepEditedEvent e) 
        {
             var step = _steps.Single(x => x.Id == e.StepId);
             step.Title = e.Title;
             step.ContentV2 = e.NewValue;
        }
    }
}
