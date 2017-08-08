using FluentAssertions;
using System;
using Ubora.Domain.Projects;
using Xunit;
using Ubora.Domain.Infrastructure;

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

            var command = new UpdateProjectImageCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = expectedProjectId,
                BlobLocation = new BlobLocation("test", "test.jpg")
            };

            // Act
            Processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(expectedProjectId);

            project.ProjectImageBlobLocation.Should().NotBeNull();
        }
    }
}
