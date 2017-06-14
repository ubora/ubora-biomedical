using System;
using System.Collections.Generic;
using Marten.Schema;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    //public class WorkpackageTwoStep
    //{
    //    public Guid Id { get; set; }
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public IEnumerable<WorkpackageTwoStepPage> Pages { get; set; }
    //}

    //public class WorkpackageTwoStepPage
    //{
    //    public Guid Id { get; set; }
    //    public Guid StepId { get; set; }
    //    public string Title { get; set; }
    //    public string Content { get; set; }
    //}

    public class WorkpackageTwo : Workpackage<WorkpackageTwo>
    {
        private void Apply(ProjectCreatedEvent e)
        {
            ProjectId = e.Id;

            Title = "TODO: Don't know yet";

            _steps.Add(new WorkpackageOneStep("Global function and subfunctions", ""));
            _steps.Add(new WorkpackageOneStep("Applicable class of medical device", ""));
            _steps.Add(new WorkpackageOneStep("Working principles and product ideas", ""));
            _steps.Add(new WorkpackageOneStep("Selection of best idea: Reaching the concept", ""));
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

            _steps.Add(new WorkpackageOneStep("Blueprints of preliminary geometries", ""));
            _steps.Add(new WorkpackageOneStep("Commercial elements", ""));
            _steps.Add(new WorkpackageOneStep("Optimization: materials, processes, performance", ""));
            _steps.Add(new WorkpackageOneStep("Prototypes and functional trials", ""));
            _steps.Add(new WorkpackageOneStep("Blueprints of optimized designs", ""));
            _steps.Add(new WorkpackageOneStep("Criteria for product approval/quality", ""));
        }
    }

    public class WorkpackageFour : Workpackage<WorkpackageFour>
    {
        private void Apply(ProjectCreatedEvent e)
        {
            ProjectId = e.Id;

            Title = "Implementation";

            _steps.Add(new WorkpackageOneStep("Prototypes and considerations for safety assessment", ""));
            _steps.Add(new WorkpackageOneStep("Results from vitro/in vivo", ""));
            _steps.Add(new WorkpackageOneStep("Additional technical documentation", ""));
            _steps.Add(new WorkpackageOneStep("Preproduction documents (including those linked to regulatory clearance)", ""));
        }

        // TODO WP4 additional
    }

    public class WorkpackageFive : Workpackage<WorkpackageFive>
    {
        private void Apply(ProjectCreatedEvent e)
        {
            ProjectId = e.Id;

            Title = "Operation";

            _steps.Add(new WorkpackageOneStep("Production documentation (final blueprints, components and plans)", ""));
            _steps.Add(new WorkpackageOneStep("Commercial documentation (WHO description sheets, flyers, user manuals)", ""));
            _steps.Add(new WorkpackageOneStep("Service considerations (suppliers, production and distribution chain, warranties? ...)", ""));
            _steps.Add(new WorkpackageOneStep("Additional operative documentation", ""));
            _steps.Add(new WorkpackageOneStep("Pilot lots", ""));
            _steps.Add(new WorkpackageOneStep("Manufacturing process validation", ""));
        }
    }
}
