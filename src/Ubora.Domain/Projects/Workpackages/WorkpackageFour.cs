using System;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageFour : Workpackage<WorkpackageFour>
    {
        public bool IsUnLocked { get; private set; }
        
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
            _steps.Add(new WorkpackageStep("IsoCompliance", "ISO compliance"));
            _steps.Add(new WorkpackageStep("ResultsFromVitroOrVivo", "Results from vitro/vivo"));
            _steps.Add(new WorkpackageStep("WP4StructuredInformationOnTheDevice", "Structured information on the device"));
            _steps.Add(new WorkpackageStep("PreproductionDocuments", "Preproduction documents"));
        }

        private void Apply(WorkpackageFourUnlockedEvent e)
        {
            IsUnLocked = true;
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
    }
}