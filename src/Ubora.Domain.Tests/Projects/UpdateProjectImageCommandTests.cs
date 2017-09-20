using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Projects;
using Xunit;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Tests.Projects
{
    public class UpdateProjectImageCommandTests : IntegrationFixture
    {
        [Fact]
        public void Command_Sets_New_Current_DateTime_To_ProjectImageLastUpdated()
        {
            var expectedProjectId = Guid.NewGuid();

            Processor.Execute(new CreateProjectCommand
            {
                Actor = new DummyUserInfo(),
                NewProjectId = expectedProjectId
            });

            var blobLocation = new BlobLocation("test", "test.jpg");
            var command = new UpdateProjectImageCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = expectedProjectId,
                BlobLocation = blobLocation
            };

            // Act
            Processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(expectedProjectId);

            project.ProjectImageBlobLocation.Should().NotBeNull();

            var @event = Session.Events.QueryRawEventDataOnly<ProjectImageUpdatedEvent>().Single();
            @event.BlobLocation.ShouldBeEquivalentTo(blobLocation);
        }
    }
}
