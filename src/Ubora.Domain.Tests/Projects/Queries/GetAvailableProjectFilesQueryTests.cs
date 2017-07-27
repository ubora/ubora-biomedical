using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Queries;
using Ubora.Domain.Projects.Repository;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Queries
{
    public class GetAvailableProjectFilesQueryTests : IntegrationFixture
    {
        [Fact]
        public void Updates_File_From_Project()
        {
            var expectedProjectId = Guid.NewGuid();
            var expectedFileId = Guid.NewGuid();
            var blobLocation = new BlobLocation("container", "path");

            var fileAddedEvent1 = new FileAddedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: expectedProjectId,
                id: expectedFileId,
                fileName: "fileName1",
                location: blobLocation);
            Session.Events.Append(expectedProjectId, fileAddedEvent1);
            Session.SaveChanges();

            var otherProjectId = Guid.NewGuid();
            var fileAddedEvent2 = new FileAddedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: otherProjectId,
                id: Guid.NewGuid(),
                fileName: "fileName2",
                location: blobLocation);
            Session.Events.Append(otherProjectId, fileAddedEvent2);
            Session.SaveChanges();

            var fileId = Guid.NewGuid();
            var fileAddedEvent3 = new FileAddedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: expectedProjectId,
                id: fileId,
                fileName: "fileName3",
                location: blobLocation);
            Session.Events.Append(expectedProjectId, fileAddedEvent3);
            Session.SaveChanges();

            var fileHidEvent = new FileHidEvent(
                initiatedBy: new DummyUserInfo(),
                id: fileId,
                fileName: "fileName3"
                );
            Session.Events.Append(expectedProjectId, fileHidEvent);
            Session.SaveChanges();

            var query = new GetAvailableProjectFilesQuery(expectedProjectId);

            // Act
            var result = Processor.ExecuteQuery(query).ToList();

            // Assert
            result.Single().Id.Should().Be(expectedFileId);
            result.Single().ProjectId.Should().Be(expectedProjectId);
            result.Single().IsHidden.Should().BeFalse();
            result.Single().FileName.Should().Be(fileAddedEvent1.FileName);
        }
    }
}
