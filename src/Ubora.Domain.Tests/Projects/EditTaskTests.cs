﻿using System;
using Autofac;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class EditTaskTests : IntegrationFixture
    {
        public EditTaskTests()
        {
            StoreOptions(new UboraStoreOptions().Configuration());
        }

        [Fact]
        public void Edits_Task_Of_Project()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var expectedProjectId = Guid.NewGuid();
            processor.Execute(new CreateProjectCommand
            {
                UserInfo = new UserInfo(Guid.NewGuid(), ""),
                ProjectId = expectedProjectId
            });

            var expectedTaskId = Guid.NewGuid();
            var expectedUserInfo = new UserInfo(Guid.NewGuid(), "");
            processor.Execute(new AddTaskCommand
            {
                Title = "initialTitle",
                Description = "initialDescription",
                ProjectId = expectedProjectId,
                Id = expectedTaskId,
                InitiatedBy = expectedUserInfo
            });

            var command2 = new EditTaskCommand
            {
                Title = "expectedTitle",
                Description = "expectedDescription",
                ProjectId = expectedProjectId,
                Id = expectedTaskId,
                InitiatedBy = expectedUserInfo
            };

            // Act
            processor.Execute(command2);

            // Assert
            var task = Session.Load<ProjectTask>(expectedTaskId);

            task.Id.Should().Be(expectedTaskId);
            task.Title.Should().Be("expectedTitle");
            task.Description.Should().Be("expectedDescription");
            task.ProjectId.Should().Be(expectedProjectId);
        }
    }
}