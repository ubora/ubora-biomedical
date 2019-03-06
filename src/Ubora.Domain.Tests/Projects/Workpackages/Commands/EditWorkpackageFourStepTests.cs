using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects._Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class EditWorkpackageFourStepTests : IntegrationFixture
    {
        [Fact]
        public void Workpackage_Four_Step_Can_Be_Edited()
        {
            var project = new ProjectSeeder()
                .WithWp1Accepted()
                .WithWp3Unlocked()
                .WithWp4Unlocked()
                .Seed(this);

            var workpackage = Session.Load<WorkpackageFour>(project.Id);

            var randomStepToEdit = workpackage.Steps.Skip(1).First();
            var initialTitle = randomStepToEdit.Title;

            var command = new EditWorkpackageFourStepCommand
            {
                ProjectId = project.Id,
                StepId = randomStepToEdit.Id,
                NewValue = new QuillDelta("{test}"),
                Actor = new DummyUserInfo()
            };

            // Act
            Processor.Execute(command);

            // Assert
            RefreshSession();

            workpackage = Session.Load<WorkpackageFour>(project.Id);
            var editedStep = workpackage.Steps.Single(x => x.Id == randomStepToEdit.Id);

            editedStep.ContentV2.Should().Be(new QuillDelta("{test}"));
            editedStep.Title.Should().Be(initialTitle);
        }
    }
}