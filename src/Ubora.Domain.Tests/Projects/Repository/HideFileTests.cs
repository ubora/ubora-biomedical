using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
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
            var expectedFileId = Guid.NewGuid();
            var fileName = "expectedFileName";

            var fileAddedEvent = new FileAddedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: expectedProjectId,
                id: expectedFileId,
                fileName: fileName,
                location: new BlobLocation("container", "blobPath"));
            Session.Events.Append(expectedProjectId, fileAddedEvent);
            Session.SaveChanges();

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
            file.FileName.Should().Be(fileName);
            file.Location.BlobPath.Should().Be("blobPath");
            file.IsHidden.Should().BeTrue();

            var fileHidEvents = Session.Events.QueryRawEventDataOnly<FileHidEvent>();

            fileHidEvents.Count().Should().Be(1);
            fileHidEvents.First().Id.Should().Be(expectedFileId);
        }
    }
}
