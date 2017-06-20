using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageTwo : Workpackage<WorkpackageTwo>
    {
        private void Apply(WorkpackageOneAcceptedByReviewEvent e)
        {
            ProjectId = e.ProjectId;

            Title = "Conceptual design";

            // TODO(Kaspar Kallas
            //_steps.Add(new WorkpackageStep("Description of functions", ""));
            //_steps.Add(new WorkpackageStep("Description of minimal requirements for safety and ISO compliance", ""));
            //_steps.Add(new WorkpackageStep("Sketches of alternate ideas and designs", ""));
            //_steps.Add(new WorkpackageStep("Selection of best idea: Reaching the concept", ""));
            //_steps.Add(new WorkpackageStep("Latest concept description (example: rendering)", ""));
        }
    }
}