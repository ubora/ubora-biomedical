using System;
using Marten.Schema;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public class WorkpackageOne : Entity<WorkpackageOne>
    {
        [Identity]
        public Guid ProjectId { get; private set; }

        public string DescriptionOfNeed { get; private set; }
        public string DescriptionOfExistingSolutionsAndAnalysis { get; private set; }
        public string ProductFunctionality { get; private set; }
        public string ProductPerformance { get; private set; }
        public string ProductUsability { get; private set; }
        public string ProductSafety { get; private set; }
        public string PatientPopulationStudy { get; private set; }
        public string UserRequirementStudy { get; private set; }
        public string AdditionalInformation { get; private set; }

        private void Apply(WorkpackageOneOpenedEvent e)
        {
            ProjectId = e.ProjectId;
        }

        private void Apply(DescriptionOfNeedEdited e)
        {
            DescriptionOfNeed = e.Value;
        }

        private void Apply(DescriptionOfExistingSolutionsAndAnalysisEditedEvent e)
        {
            DescriptionOfExistingSolutionsAndAnalysis = e.Value;
        }

        private void Apply(ProductFunctionalityEditedEvent e)
        {
            ProductFunctionality = e.Value;
        }

        private void Apply(ProductPerformanceEditedEvent e)
        {
            ProductPerformance = e.Value;
        }

        private void Apply(ProductUsabilityEditedEvent e)
        {
            ProductUsability = e.Value;
        }

        private void Apply(ProductSafetyEditedEvent e)
        {
            ProductSafety = e.Value;
        }

        private void Apply(PatientPopulationStudyEditedEvent e)
        {
            PatientPopulationStudy = e.Value;
        }

        private void Apply(UserRequirementStudyEditedEvent e)
        {
            UserRequirementStudy = e.Value;
        }

        private void Apply(AdditionalInformationEditedEvent e)
        {
            AdditionalInformation = e.Value;
        }
    }
}