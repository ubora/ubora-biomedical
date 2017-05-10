using System;
using Autofac;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class AddTaskTests : IntegrationFixture
    {
        public AddTaskTests()
        {
            StoreOptions(new UboraStoreOptions().Configuration());
        }

        [Fact]
        public void Adds_New_Task_To_Project()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var expectedProjectId = Guid.NewGuid();
            processor.Execute(new CreateProjectCommand
            {
                UserInfo = new UserInfo(Guid.NewGuid(), ""),
                Id = expectedProjectId
            });

            var expectedTaskId = Guid.NewGuid();
            var expectedUserInfo = new UserInfo(Guid.NewGuid(), "");
            var command = new AddTaskCommand
            {
                Title = "expectedTitle",
                Description = "expectedDescription",
                ProjectId = expectedProjectId,
                Id = expectedTaskId,
                UserInfo = expectedUserInfo
            };

            // Act
            processor.Execute(command);

            // Assert
            var task = Session.Load<ProjectTask>(expectedTaskId);

            task.Id.Should().Be(expectedTaskId);
            task.Title.Should().Be("expectedTitle");
            task.Description.Should().Be("expectedDescription");
            task.ProjectId.Should().Be(expectedProjectId);
        }
    }
}
