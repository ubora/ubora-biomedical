using System;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageFour : Workpackage<WorkpackageFour>
    {       
        private void Apply(WorkpackageFourOpenedEvent e)
        {
            if (_steps.Any())
            {
                throw new InvalidOperationException("Already opened.");         
            }

            ProjectId = e.ProjectId;

            Title = "Implementation";
            
            _steps.Add(new WorkpackageStep("PrototypesAndConsiderationsForSafetyAssessment", "Prototypes and considerations for safety assessment"));
            _steps.Add(new WorkpackageStep("QualityCriteria", "Quality criteria"));
            _steps.Add(new WorkpackageStep("ResultsFromVitroOrVivo", "Results from vitro/vivo"));
        }
      
        private void Apply(WorkpackageFourStepEdited e)
        {
            var step = GetSingleStep(e.StepId);
            if (step == null)
            {
                throw new InvalidOperationException($"{nameof(WorkpackageStep)} not found with id [{e.StepId}]");
            }
            
            step.Title = e.Title;
            step.Content = e.NewValue;
        }

        [Obsolete]
        private void Apply(WorkpackageFourStepEditedEventV2 e)
        {
            var step = _steps.Single(x => x.Id == e.StepId);

            step.Title = e.Title;
            step.ContentV2 = e.NewValue;
        }
    }
}