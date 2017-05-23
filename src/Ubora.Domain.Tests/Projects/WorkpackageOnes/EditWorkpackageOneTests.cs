using System;
using FluentAssertions;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.WorkpackageOnes;
using Xunit;

namespace Ubora.Domain.Tests.Projects.WorkpackageOnes
{
    public class EditWorkpackageOneTests : IntegrationFixture
    {
        [Fact]
        public void Project_Workpackage_One_Can_Be_Edited()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new DummyUserInfo()
            });

            // Act
            ExecuteEditWorkpackageOneCommand<EditDescriptionOfNeedCommand>(projectId, value: nameof(EditDescriptionOfNeedCommand));
            ExecuteEditWorkpackageOneCommand<EditDescriptionOfExistingSolutionsAndAnalysisCommand>(projectId, value: nameof(EditDescriptionOfExistingSolutionsAndAnalysisCommand));
            ExecuteEditWorkpackageOneCommand<EditProductFunctionalityCommand>(projectId, value: nameof(EditProductFunctionalityCommand));
            ExecuteEditWorkpackageOneCommand<EditProductPerformanceCommand>(projectId, value: nameof(EditProductPerformanceCommand));
            ExecuteEditWorkpackageOneCommand<EditProductUsabilityCommand>(projectId, value: nameof(EditProductUsabilityCommand));
            ExecuteEditWorkpackageOneCommand<EditProductSafetyCommand>(projectId, value: nameof(EditProductSafetyCommand));
            ExecuteEditWorkpackageOneCommand<EditPatientPopulationStudyCommand>(projectId, value: nameof(EditPatientPopulationStudyCommand));
            ExecuteEditWorkpackageOneCommand<EditUserRequirementStudyCommand>(projectId, value: nameof(EditUserRequirementStudyCommand));
            ExecuteEditWorkpackageOneCommand<EditAdditionalInformationCommand>(projectId, value: nameof(EditAdditionalInformationCommand));

            // Assert
            var workpackage = Session.Load<WorkpackageOne>(projectId);

            workpackage.DescriptionOfNeed.Should().Be(nameof(EditDescriptionOfNeedCommand));
            workpackage.DescriptionOfExistingSolutionsAndAnalysis.Should().Be(nameof(EditDescriptionOfExistingSolutionsAndAnalysisCommand));
            workpackage.ProductFunctionality.Should().Be(nameof(EditProductFunctionalityCommand));
            workpackage.ProductPerformance.Should().Be(nameof(EditProductPerformanceCommand));
            workpackage.ProductUsability.Should().Be(nameof(EditProductUsabilityCommand));
            workpackage.ProductSafety.Should().Be(nameof(EditProductSafetyCommand));
            workpackage.PatientPopulationStudy.Should().Be(nameof(EditPatientPopulationStudyCommand));
            workpackage.UserRequirementStudy.Should().Be(nameof(EditUserRequirementStudyCommand));
            workpackage.AdditionalInformation.Should().Be(nameof(EditAdditionalInformationCommand));
        }

        private void ExecuteEditWorkpackageOneCommand<T>(Guid projectId, string value) where T : EditWorkpackageOneCommand, new()
        {
            Processor.Execute(new T
            {
                ProjectId = projectId,
                NewValue = value,
                Actor = new DummyUserInfo()
            });
        }
    }
}
