﻿using System;
using Autofac;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Projects.Tasks.Commands;
using Ubora.Domain.Projects._Commands;
using Xunit;
using System.Linq;
using Ubora.Domain.Projects.Tasks.Notifications;

namespace Ubora.Domain.Tests.Projects.Tasks.Commands
{
    public class EditTaskTests : IntegrationFixture
    {
        [Fact]
        public void Edits_Task_Of_Project()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var expectedProjectId = Guid.NewGuid();
            processor.Execute(new CreateProjectCommand
            {
                Actor = new UserInfo(Guid.NewGuid(), ""),
                NewProjectId = expectedProjectId
            });

            var expectedTaskId = Guid.NewGuid();
            var expectedUserInfo = new UserInfo(Guid.NewGuid(), "");
            var taskAssignee1Id = Guid.NewGuid();
            processor.Execute(new AddTaskCommand
            {
                Title = "initialTitle",
                Description = "initialDescription",
                ProjectId = expectedProjectId,
                Id = expectedTaskId,
                Actor = expectedUserInfo,
                AssigneeIds = new[] { taskAssignee1Id }
            });

            var taskAssignee2Id = Guid.NewGuid();
            var taskAssignee3Id = Guid.NewGuid();
            var editedTaskAssigneeIds = new[] { taskAssignee2Id, taskAssignee3Id };
            var command2 = new EditTaskCommand
            {
                Title = "expectedTitle",
                Description = "expectedDescription",
                ProjectId = expectedProjectId,
                Id = expectedTaskId,
                Actor = expectedUserInfo,
                AssigneeIds = editedTaskAssigneeIds
            };

            // Act
            processor.Execute(command2);

            // Assert
            var task = Session.Load<ProjectTask>(expectedTaskId);

            task.Id.Should().Be(expectedTaskId);
            task.Title.Should().Be("expectedTitle");
            task.Description.Should().Be("expectedDescription");
            task.ProjectId.Should().Be(expectedProjectId);
            task.Assignees.Count().Should().Be(2);
            task.Assignees.First().As<TaskAssignee>().UserId.Should().Be(taskAssignee2Id);
            task.Assignees.Last().As<TaskAssignee>().UserId.Should().Be(taskAssignee3Id);

            var assignmentAssignedNotifications = Session.Query<AssignmentAssignedToNotification>();
            assignmentAssignedNotifications.Count().Should().Be(3);
            assignmentAssignedNotifications.Single(x => x.NotificationTo == taskAssignee1Id).TaskId.Should().Be(expectedTaskId);
            assignmentAssignedNotifications.Single(x => x.NotificationTo == taskAssignee2Id).TaskId.Should().Be(expectedTaskId);
            assignmentAssignedNotifications.Single(x => x.NotificationTo == taskAssignee3Id).TaskId.Should().Be(expectedTaskId);

            var assignmentRemovedNotifications = Session.Query<AssignmentRemovedFromNotification>();
            assignmentRemovedNotifications.Count().Should().Be(1);
            assignmentRemovedNotifications.Single(x => x.NotificationTo == taskAssignee1Id).TaskId.Should().Be(expectedTaskId);
        }
    }
}
