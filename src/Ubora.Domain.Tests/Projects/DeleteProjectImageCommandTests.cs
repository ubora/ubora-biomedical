using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects._Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class DeleteProjectImageCommandTests : IntegrationFixture
    {
        [Fact]
        public void Command_Sets_Null_To_ProjectImageLastUpdated()
        {
            var expectedProjectId = Guid.NewGuid();

            Processor.Execute(new CreateProjectCommand
            {
                Actor = new DummyUserInfo(),
                NewProjectId = expectedProjectId
            });

            Processor.Execute(new UpdateProjectImageCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = expectedProjectId
            });

            var command = new DeleteProjectImageCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = expectedProjectId
            };

            // Act
            Processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(expectedProjectId);

            project.ProjectImageBlobLocation.Should().BeNull();

            var @event = Session.Events.QueryRawEventDataOnly<ProjectImageDeletedEvent>().Single();
            @event.Should().NotBeNull();
        }
    }
}
