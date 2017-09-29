using System;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Commands
{
    public class UpdateProjectDescriptionTests : IntegrationFixture
    {
        [Fact]
        public void Updates_Project_Description()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var command = new UpdateProjectDescriptionCommand
            {
                ProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                Description = "description"
            };

            // Act
            Processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(projectId);

            project.Description.Should().Be("description");
        }
    }
}
