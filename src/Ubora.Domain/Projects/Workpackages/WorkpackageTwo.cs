using System.Linq;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageTwo : Workpackage<WorkpackageTwo>
    {
        private void Apply(WorkpackageOneAcceptedByReviewEvent e)
        {
            ProjectId = e.ProjectId;

            Title = "Conceptual design";

            _steps.Add(new WorkpackageStep(WorkpackageStepIds.DescriptionOfFunctions, "Description of Functions", ""));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.DescriptionOfMinimalRequirementsForSafetyAndIsoCompliance, "Description of Minimal Requirements for Safety and ISO Compliance", ""));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.SketchesOfAlternateIdeasAndDesigns, "Sketches of Alternate Ideas and Designs", ""));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.SelectionOfBestIdea, "Selection of Best Idea: Reaching the Concept", ""));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.LatestConceptDescription, "Latest Concept Description (example: rendering)", ""));
        }

        private void Apply(WorkpackageTwoStepEdited e)
        {
            var step = _steps.Single(x => x.Id == e.StepId);

            step.Title = e.Title;
            step.Content = e.NewValue;
        }
    }
}