using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects.Assignments;
using Ubora.Domain.Projects.Assignments.Commands;
using Ubora.Domain.Projects.Assignments.Notifications;
using Ubora.Domain.Projects._Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Assignments.Commands
{
    public class AddAssignmentTests : IntegrationFixture
    {
        [Fact]
        public void Adds_New_Task_To_Project()
        {
            var expectedProjectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                Actor = new DummyUserInfo(),
                NewProjectId = expectedProjectId
            });

            var expectedTaskId = Guid.NewGuid();
            var taskAssigneeId = Guid.NewGuid();
            var assigneeIds = new[] { taskAssigneeId };
            var actor = new DummyUserInfo();
            var command = new AddAssignmentCommand
            {
                Title = "expectedTitle",
                Description = "expectedDescription",
                ProjectId = expectedProjectId,
                Id = expectedTaskId,
                Actor = actor,
                AssigneeIds = assigneeIds
            };

            // Act
            Processor.Execute(command);

            // Assert
            var task = Session.Load<Assignment>(expectedTaskId);

            task.Id.Should().Be(expectedTaskId);
            task.Title.Should().Be("expectedTitle");
            task.Description.Should().Be("expectedDescription");
            task.ProjectId.Should().Be(expectedProjectId);
            task.Assignees.Single().As<TaskAssignee>().UserId.Should().Be(taskAssigneeId);

            var storedNotification = Session.Query<AssignmentAssignedToNotification>().Single();
            storedNotification.NotificationTo.Should().Be(taskAssigneeId);
            storedNotification.ProjectId.Should().Be(expectedProjectId);
            storedNotification.TaskId.Should().Be(expectedTaskId);
            storedNotification.RequesterId.Should().Be(actor.UserId);
            storedNotification.GetDescription().Should().Be($"Assignment {StringTokens.Task(expectedTaskId)} was assigned to you!");
        }
    }
}
