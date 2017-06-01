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

        public string Title { get; private set; }

        [JsonProperty(nameof(Steps))]
        private readonly HashSet<WorkpackageOneStep> _steps = new HashSet<WorkpackageOneStep>();

        [JsonIgnore]
        public IReadOnlyCollection<WorkpackageOneStep> Steps => _steps;

        private void Apply(WorkpackageOneOpenedEvent e)
        {
            ProjectId = e.ProjectId;
            Title = "Design and prototyping";
            _steps.Add(new WorkpackageOneStep("Description Of Need", Placeholders.DescriptionOfNeed));
            _steps.Add(new WorkpackageOneStep("Description Of Existing Solutions And Analysis", Placeholders.DescriptionOfExistingSolutionsAndAnalysis));
            _steps.Add(new WorkpackageOneStep("Product Functionality", Placeholders.ProductFunctionality));
            _steps.Add(new WorkpackageOneStep("Product Performance", Placeholders.ProductPerformance));
            _steps.Add(new WorkpackageOneStep("Product Usability", Placeholders.ProductUsability));
            _steps.Add(new WorkpackageOneStep("Product Safety", Placeholders.ProductSafety));
            _steps.Add(new WorkpackageOneStep("Patient Population Study", Placeholders.PatientPopulationStudy));
            _steps.Add(new WorkpackageOneStep("User Requirement Study", Placeholders.UserRequirementStudy));
            _steps.Add(new WorkpackageOneStep("Additional Information", Placeholders.AdditionalInformation));
        }

        private void Apply(WorkpackageOneStepEditedEvent e)
        {
            var step = Steps.Single(x => x.Id == e.StepId);

            step.Title = e.Title;
            step.Content = e.NewValue;
        }

        // Virtual for testing
        public virtual WorkpackageOneStep GetSingleStep(Guid stepId)
        {
            return _steps.Single(step => step.Id == stepId);
        }
    }
}