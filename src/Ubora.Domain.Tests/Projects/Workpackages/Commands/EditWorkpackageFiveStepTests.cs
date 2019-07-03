using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects._Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class EditWorkpackageFiveStepTests : IntegrationFixture
    {
        [Fact]
        public void Workpackage_Five_Step_Can_Be_Edited()
        {
            var project = new ProjectSeeder()
                .WithWp1Accepted()
                .WithWp3Unlocked()
                .WithWp4Unlocked()
                .WithWp5Unlocked()
                .Seed(this);

            var workpackage = Session.Load<WorkpackageFive>(project.Id);

            var randomStepToEdit = workpackage.Steps.First();
            var initialTitle = randomStepToEdit.Title;

            var command = new EditWorkpackageFiveStepCommand
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

            workpackage = Session.Load<WorkpackageFive>(project.Id);
            var editedStep = workpackage.Steps.Single(x => x.Id == randomStepToEdit.Id);

            editedStep.ContentV2.Should().Be(new QuillDelta("{test}"));
            editedStep.Title.Should().Be(initialTitle);
        }
    }
}