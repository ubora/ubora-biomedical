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
    public class IsHiddenFileSpecTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_ProjectFiles_Which_Are_Hidden()
        {
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

            Processor.Execute(new AddFileCommand
            {
                ProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new HideFileCommand
            {
                ProjectId = projectId,
                Id = expectedFileId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var sut = new IsHiddenFileSpec();

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
