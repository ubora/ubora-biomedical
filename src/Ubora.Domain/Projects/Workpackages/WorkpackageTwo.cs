using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageTwo : Workpackage<WorkpackageTwo>
    {
        private void Apply(ProjectCreatedEvent e)
        {
            ProjectId = e.Id;

            Title = "TODO: Don't know yet";

            _steps.Add(new WorkpackageStep("Global function and subfunctions", ""));
            _steps.Add(new WorkpackageStep("Applicable class of medical device", ""));
            _steps.Add(new WorkpackageStep("Working principles and product ideas", ""));
            _steps.Add(new WorkpackageStep("Selection of best idea: Reaching the concept", ""));
        }

        private void Apply(WorkpackageOneAcceptedByReviewEvent e)
        {
            IsVisible = true;
        }


    }

    public class WorkpackageThree : Workpackage<WorkpackageThree>
    {
        private void Apply(ProjectCreatedEvent e)
        {
            ProjectId = e.Id;

            Title = "Design and prototyping";

            _steps.Add(new WorkpackageStep("Blueprints of preliminary geometries", ""));
            _steps.Add(new WorkpackageStep("Commercial elements", ""));
            _steps.Add(new WorkpackageStep("Optimization: materials, processes, performance", ""));
            _steps.Add(new WorkpackageStep("Prototypes and functional trials", ""));
            _steps.Add(new WorkpackageStep("Blueprints of optimized designs", ""));
            _steps.Add(new WorkpackageStep("Criteria for product approval/quality", ""));
        }
    }

    public class WorkpackageFour : Workpackage<WorkpackageFour>
    {
        private void Apply(ProjectCreatedEvent e)
        {
            ProjectId = e.Id;

            Title = "Implementation";

            _steps.Add(new WorkpackageStep("Prototypes and considerations for safety assessment", ""));
            _steps.Add(new WorkpackageStep("Results from vitro/in vivo", ""));
            _steps.Add(new WorkpackageStep("Additional technical documentation", ""));
            _steps.Add(new WorkpackageStep("Preproduction documents (including those linked to regulatory clearance)", ""));
        }

        // TODO WP4 additional
    }

    public class WorkpackageFive : Workpackage<WorkpackageFive>
    {
        private void Apply(ProjectCreatedEvent e)
        {
            ProjectId = e.Id;

            Title = "Operation";

            _steps.Add(new WorkpackageStep("Production documentation (final blueprints, components and plans)", ""));
            _steps.Add(new WorkpackageStep("Commercial documentation (WHO description sheets, flyers, user manuals)", ""));
            _steps.Add(new WorkpackageStep("Service considerations (suppliers, production and distribution chain, warranties? ...)", ""));
            _steps.Add(new WorkpackageStep("Additional operative documentation", ""));
            _steps.Add(new WorkpackageStep("Pilot lots", ""));
            _steps.Add(new WorkpackageStep("Manufacturing process validation", ""));
        }
    }
}
