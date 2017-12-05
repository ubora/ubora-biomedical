using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class EditWorkpackageThreeStepTests : IntegrationFixture
    {
        [Fact]
        public void Workpackage_Three_Step_Can_Be_Edited()
        {
            var projectId = Guid.NewGuid();
            var editCommand = new EditWorkpackageThreeStepCommand
            {
                ProjectId = projectId,
                StepId = "GeneralProductDescription_ElectronicAndFirmware_PurposelyDesignedParts",
                NewValue = "testValue",
                Actor = new DummyUserInfo()
            };

            this.Given(_ => this.Create_Project(projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(projectId))
                    .And(_ => this.Accept_Workpackage_One_Review(projectId))
                .When(_ => Processor.Execute(editCommand))
                .Then(_ => Assert_WP3_Step_Has_Value(projectId, editCommand.StepId, editCommand.NewValue))
                .BDDfy();
        }

        private void Assert_WP3_Step_Has_Value(Guid projectId, string stepId, string contentValue)
        {
            var wp3 = Processor.FindById<WorkpackageThree>(projectId);

            wp3.Steps.Single(s => s.Id == stepId).Content.Should().Be(contentValue);
        }
    }
}