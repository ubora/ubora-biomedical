using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.StructuredInformations
{
    public class EditHealthTechnologySpecificationInformationCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Edits_Health_Technology_Specification_Information_Of_Device()
        {
            var projectId = Guid.NewGuid();
            var healthTechnologySpecificationsInformation = HealthTechnologySpecificationsInformation.CreateEmpty();

            this.Given(_ => this.Create_Project(projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(projectId))
                    .And(_ => this.Accept_Workpackage_One_Review(projectId))
                .When(_ => Execute_Command_Under_Test(projectId, healthTechnologySpecificationsInformation))
                .Then(_ => Assert_Structured_Information_Aggregate_Has_Edited_Data(projectId, healthTechnologySpecificationsInformation))
                .BDDfy();
        }

        private void Execute_Command_Under_Test(Guid projectId, HealthTechnologySpecificationsInformation healthTechnologySpecificationsInformation)
        {
            var commandUnderTest = new EditHealthTechnologySpecificationInformationCommand
            {
                HealthTechnologySpecificationsInformation = healthTechnologySpecificationsInformation,
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            };

            Processor.Execute(commandUnderTest);
        }

        private void Assert_Structured_Information_Aggregate_Has_Edited_Data(Guid projectId, HealthTechnologySpecificationsInformation healthTechnologySpecificationsInformation)
        {
            var aggregate = Processor.FindById<DeviceStructuredInformation>(projectId);

            aggregate.HealthTechnologySpecification.ShouldBeEquivalentTo(healthTechnologySpecificationsInformation);
        }
    }
}
