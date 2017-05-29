using System;
using System.Collections.Generic;
using System.Linq;
using Marten.Schema;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public class WorkpackageOne : Entity<WorkpackageOne>
    {
        [Identity]
        public Guid ProjectId { get; private set; }

        public string Title { get; set; }

        public ICollection<WorkpackageOneStep> Steps { get; set; }

        private void Apply(WorkpackageOneOpenedEvent e)
        {
            ProjectId = e.ProjectId;
            Title = "Design and prototyping";
            Steps = new[]
            {
                new WorkpackageOneStep("Description Of Need", ""),
                new WorkpackageOneStep("Description Of Existing Solutions And Analysis", ""),
                new WorkpackageOneStep("Product Functionality", ""),
                new WorkpackageOneStep("Product Performance", ""),
                new WorkpackageOneStep("Product Usability", ""),
                new WorkpackageOneStep("Product Safety", ""),
                new WorkpackageOneStep("Patient Population Study", ""),
                new WorkpackageOneStep("User Requirement Study", ""),
                new WorkpackageOneStep("Additional Information", "")
            };
        }

        private void Apply(WorkpackageOneStepEditedEvent e)
        {
            var step = Steps.Single(x => x.Id == e.StepId);

            step.Title = e.Title;
            step.Value = e.NewValue;
        }
    }
}