using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
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
            var expectedProjectId = Guid.NewGuid();
            var expectedTaskId = Guid.NewGuid();
            var expectedUserInfo = new UserInfo(Guid.NewGuid(), "");
            var command = new AddTaskCommand
            {
                Title = "expectedTitle",
                Description = "expectedDescription",
                ProjectId = expectedProjectId,
                TaskId = expectedTaskId,
                InitiatedBy = expectedUserInfo
            };

            var processor = Container.Resolve<ICommandProcessor>();

            // Act
            processor.Execute(command);

            // Assert

            // A:
            var e = Session.Events.FetchStream(expectedProjectId).Select(x => x.Data).OfType<TaskAddedEvent>().Single();

            e.Title.Should().Be("expectedTitle");
            e.Description.Should().Be("expectedDescription");
            e.ProjectId.Should().Be(expectedProjectId);
            e.TaskId.Should().Be(expectedTaskId);

            // B:
            var task = Session.Load<ProjectTask>(expectedTaskId);

            task.Id.Should().Be(expectedTaskId);
            task.Title.Should().Be("expectedTitle");
            task.Description.Should().Be("expectedDescription");
            task.ProjectId.Should().Be(expectedProjectId);
        }
    }
}
