using System;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Repository;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Repository
{
    public class HideFileTests : IntegrationFixture
    {
        [Fact]
        public void Hides_File_From_Project()
        {
            var expectedProjectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                Actor = new DummyUserInfo(),
                NewProjectId = expectedProjectId
            });

            var expectedFileId = Guid.NewGuid();
            Processor.Execute(new AddFileCommand
            {
                FileName = "expectedFileName",
                ProjectId = expectedProjectId,
                Id = expectedFileId,
                Actor = new DummyUserInfo(),
                BlobLocation = new BlobLocation("container", "blobPath")
            });

            var command = new HideFileCommand
            {
                Id = expectedFileId,
                Actor = new DummyUserInfo()
            };

            // Act
            Processor.Execute(command);

            // Assert
            var file = Session.Load<ProjectFile>(expectedFileId);

            file.Id.Should().Be(expectedFileId);
            file.ProjectId.Should().Be(expectedProjectId);
            file.FileName.Should().Be("expectedFileName");
            file.Location.BlobPath.Should().Be("blobPath");
            file.IsHidden.Should().BeTrue();
        }
    }
}
