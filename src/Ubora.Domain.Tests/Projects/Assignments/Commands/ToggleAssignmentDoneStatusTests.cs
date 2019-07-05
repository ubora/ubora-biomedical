using FluentAssertions;
using System;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects.Assignments;
using Ubora.Domain.Projects.Assignments.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Assignments.Commands
{
    public class ToggleAssignmentDoneStatusTests : IntegrationFixture
    {
        [Fact]
        public void Marks_Assignment_As_Done_If_Was_Not_Done()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                Actor = new DummyUserInfo(),
                NewProjectId = projectId
            });

            var assignmentId = Guid.NewGuid();
            var assigneeId = Guid.NewGuid();
            Processor.Execute(new AddAssignmentCommand
            {
                Title = "expectedTitle",
                Description = "expectedDescription",
                ProjectId = projectId,
                Id = assignmentId,
                Actor = new DummyUserInfo(),
                AssigneeIds = new[] { assigneeId }
            });

            var toggleCommand = new ToggleAssignmentDoneStatusCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = projectId,
                Id = assignmentId
            };

            // Act
            var result = Processor.Execute(toggleCommand);

            // Assert
            var resultAssignment = Processor.FindById<Assignment>(assignmentId);
            resultAssignment.IsDone.Should().BeTrue();
        }

        [Fact]
        public void Marks_Assignment_As_Not_Done_If_Was_Done()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                Actor = new DummyUserInfo(),
                NewProjectId = projectId
            });

            var assignmentId = Guid.NewGuid();
            var assigneeId = Guid.NewGuid();
            Processor.Execute(new AddAssignmentCommand
            {
                Title = "expectedTitle",
                Description = "expectedDescription",
                ProjectId = projectId,
                Id = assignmentId,
                Actor = new DummyUserInfo(),
                AssigneeIds = new[] { assigneeId }
            });

            var toggleCommand = new ToggleAssignmentDoneStatusCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = projectId,
                Id = assignmentId
            };

            Processor.Execute(toggleCommand);

            // Act
            var result = Processor.Execute(toggleCommand);

            // Assert
            var resultAssignment = Processor.FindById<Assignment>(assignmentId);
            resultAssignment.IsDone.Should().BeFalse();
        }
    }
}
