using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageThree : Workpackage<WorkpackageThree>
    {
        private void Apply(WorkpackageTwoSubmittedForReviewEvent e)
        {
            ProjectId = e.ProjectId;

            Title = "Design and prototyping";
        }
    }
}