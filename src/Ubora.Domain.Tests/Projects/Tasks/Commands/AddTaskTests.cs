using System;
using FluentAssertions;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Projects.Tasks.Commands;
using Ubora.Domain.Projects._Commands;
using Xunit;
using System.Linq;
using Ubora.Domain.Projects.Tasks.Notifications;

namespace Ubora.Domain.Tests.Projects.Tasks.Commands
{
    public class AddTaskTests : IntegrationFixture
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
            var command = new AddTaskCommand
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
            var task = Session.Load<ProjectTask>(expectedTaskId);

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
