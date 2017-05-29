using System;
using System.Collections.Generic;
using System.Linq;
using Marten.Schema;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public class WorkpackageOne : Entity<WorkpackageOne>
    {
        [Identity]
        public Guid ProjectId { get; private set; }

        public string Title { get; set; }

        [JsonProperty(nameof(Members))]
        private readonly HashSet<WorkpackageOneStep> _steps = new HashSet<WorkpackageOneStep>();

        [JsonIgnore]
        public IReadOnlyCollection<WorkpackageOneStep> Steps => _steps;

        private void Apply(WorkpackageOneOpenedEvent e)
        {
            ProjectId = e.ProjectId;
            Title = "Design and prototyping";
            _steps.Add(new WorkpackageOneStep("Description Of Need", ""));
            _steps.Add(new WorkpackageOneStep("Description Of Existing Solutions And Analysis", ""));
            _steps.Add(new WorkpackageOneStep("Product Functionality", ""));
            _steps.Add(new WorkpackageOneStep("Product Performance", ""));
            _steps.Add(new WorkpackageOneStep("Product Usability", ""));
            _steps.Add(new WorkpackageOneStep("Product Safety", ""));
            _steps.Add(new WorkpackageOneStep("Patient Population Study", ""));
            _steps.Add(new WorkpackageOneStep("User Requirement Study", ""));
            _steps.Add(new WorkpackageOneStep("Additional Information", ""));
        }

        private void Apply(WorkpackageOneStepEditedEvent e)
        {
            var step = Steps.Single(x => x.Id == e.StepId);

            step.Title = e.Title;
            step.Value = e.NewValue;
        }
    }
}