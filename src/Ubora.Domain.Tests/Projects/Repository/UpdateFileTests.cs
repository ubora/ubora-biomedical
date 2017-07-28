using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Repository
{
    public class UpdateFileTests : IntegrationFixture
    {
        [Fact]
        public void Updates_File_From_Project()
        {
            var expectedProjectId = Guid.NewGuid();
            var expectedFileId = Guid.NewGuid();
            var expectedFileName = "fileName";
            var blobLocation = new BlobLocation("container", "path");

            var fileAddedEvent = new FileAddedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: expectedProjectId,
                id: expectedFileId,
                fileName: expectedFileName,
                location: blobLocation);
            Session.Events.Append(expectedProjectId, fileAddedEvent);
            Session.SaveChanges();

            var expectedBlobLocation = new BlobLocation("expectedContainer", "expectedBlobPath");
            var updateFileCommand = new UpdateFileCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = expectedProjectId,
                Id = expectedFileId,
                BlobLocation = expectedBlobLocation
            };

            // Act
            Processor.Execute(updateFileCommand);

            // Assert
            var file = Session.Load<ProjectFile>(expectedFileId);

            file.Id.Should().Be(expectedFileId);
            file.ProjectId.Should().Be(expectedProjectId);
            file.FileName.Should().Be(expectedFileName);
            file.Location.Should().Be(expectedBlobLocation);
            file.IsHidden.Should().BeFalse();

            var fileUpdateEvents = Session.Events.QueryRawEventDataOnly<FileUpdatedEvent>();

            fileUpdateEvents.Count().Should().Be(1);
            fileUpdateEvents.First().Id.Should().Be(expectedFileId);
            fileUpdateEvents.First().ProjectId.Should().Be(expectedProjectId);
            fileUpdateEvents.First().Location.BlobPath.Should().Be("expectedBlobPath");
        }
    }
}
