using System;
using FluentAssertions;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Xunit;

namespace Ubora.Domain.Tests.Projects
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
            var command = new AddTaskCommand
            {
                Title = "expectedTitle",
                Description = "expectedDescription",
                ProjectId = expectedProjectId,
                Id = expectedTaskId,
                Actor = new DummyUserInfo()
            };

            // Act
            Processor.Execute(command);

            // Assert
            var task = Session.Load<ProjectTask>(expectedTaskId);

            task.Id.Should().Be(expectedTaskId);
            task.Title.Should().Be("expectedTitle");
            task.Description.Should().Be("expectedDescription");
            task.ProjectId.Should().Be(expectedProjectId);
        }
    }
}
