using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.StructuredInformations
{
    public class EditInformationOnUserAndEnvironmentCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Edits_User_And_Environment_Information_Of_Device()
        {
            var projectId = Guid.NewGuid();
            var userAndEnvironmentInformation = UserAndEnvironmentInformation.CreateEmpty();

            this.Given(_ => this.Create_Project(projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(projectId))
                    .And(_ => this.Accept_Workpackage_One_Review(projectId))
                .When(_ => Execute_Command_Under_Test(projectId, userAndEnvironmentInformation))
                .Then(_ => Assert_Structured_Information_Aggregate_Has_Edited_Data(projectId, userAndEnvironmentInformation))
                .BDDfy();
        }

        private void Execute_Command_Under_Test(Guid projectId, UserAndEnvironmentInformation userAndEnvironmentInformation)
        {
            var commandUnderTest = new EditUserAndEnvironmentInformationCommand
            {
                UserAndEnvironmentInformation = userAndEnvironmentInformation,
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            };

            Processor.Execute(commandUnderTest);
        }

        private void Assert_Structured_Information_Aggregate_Has_Edited_Data(Guid projectId, UserAndEnvironmentInformation userAndEnvironmentInformation)
        {
            var aggregate = Processor.FindById<DeviceStructuredInformation>(projectId);

            aggregate.UserAndEnvironment.ShouldBeEquivalentTo(userAndEnvironmentInformation);
            aggregate.IsUserAndEnvironmentEdited.Should().BeTrue();
        }
    }
}
