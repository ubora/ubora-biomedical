using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Specifications
{
    public class IsProjectFileSpecTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_ProjectFiles_Which_Are_From_Project()
        {
            var otherProjectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = otherProjectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var expectedFileId = Guid.NewGuid();
            Processor.Execute(new AddFileCommand
            {
                ProjectId = projectId,
                Id = expectedFileId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
            });

            // Insert file to other project
            Processor.Execute(new AddFileCommand
            {
                ProjectId = otherProjectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var sut = new IsProjectFileSpec(projectId);

            RefreshSession();

            var projectFilesQueryable = Session.Query<ProjectFile>();

            // Act
            var result = sut.SatisfyEntitiesFrom(projectFilesQueryable);

            // Assert
            var projectFile = result.Single();
            projectFile.Id.Should().Be(expectedFileId);
        }
    }
}
