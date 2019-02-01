using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects._Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class EditWorkpackageOneStepTests : IntegrationFixture
    {
        [Fact]
        public void Workpackage_One_Step_Can_Be_Edited()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new DummyUserInfo()
            });

            var workpackage = Session.Load<WorkpackageOne>(projectId);

            var randomStepToEdit = workpackage.Steps.Skip(1).First();
            var initialTitle = randomStepToEdit.Title;

            var editWorkpackageOneStepCommand = new EditWorkpackageOneStepCommand
            {
                ProjectId = projectId,
                StepId = randomStepToEdit.Id,
                NewValue = new QuillDelta("{test}"),
                Actor = new DummyUserInfo()
            };

            // Act
            Processor.Execute(editWorkpackageOneStepCommand);

            // Assert
            RefreshSession();

            workpackage = Session.Load<WorkpackageOne>(projectId);
            var editedStep = workpackage.Steps.Single(x => x.Id == randomStepToEdit.Id);

            editedStep.ContentV2.Should().Be(new QuillDelta("{test}"));
            editedStep.Title.Should().Be(initialTitle);
        }
    }
}