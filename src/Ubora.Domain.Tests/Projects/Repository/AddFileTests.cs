using System;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Repository;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Repository
{
    public class AddFileTests : IntegrationFixture
    {
        [Fact]
        public void Adds_New_File_To_Project()
        {
            var expectedProjectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                Actor = new DummyUserInfo(),
                NewProjectId = expectedProjectId
            });

            var expectedFileId = Guid.NewGuid();
            var command = new AddFileCommand
            {
                FileName = "expectedFileName",
                ProjectId = expectedProjectId,
                Id = expectedFileId,
                Actor = new DummyUserInfo(),
                BlobLocation = new BlobLocation("container", "blobPath")
            };

            // Act
            Processor.Execute(command);

            // Assert
            var file = Session.Load<ProjectFile>(expectedFileId);

            file.Id.Should().Be(expectedFileId);
            file.ProjectId.Should().Be(expectedProjectId);
            file.FileName.Should().Be("expectedFileName");
            file.Location.BlobPath.Should().Be("blobPath");
            file.IsHidden.Should().BeFalse();
        }
    }
}
