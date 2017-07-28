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
        public void GetAvailableProjectFilesQuery_Returns_Project_Files_That_Are_Not_Hidden()
        {
            var expectedProjectId = Guid.NewGuid();
            var expectedFileId = Guid.NewGuid();
            var expectedFileName = "expectedFileName";

            AppendFileAddedEventToSession(expectedProjectId, expectedFileId, expectedFileName);
            
            AppendFileAddedEventToSession(projectId: Guid.NewGuid(), fileId: Guid.NewGuid(), fileName: "fileFromOtherProject");

            var hiddenFileId = Guid.NewGuid();
            AppendFileAddedEventToSession(projectId: expectedProjectId, fileId: hiddenFileId, fileName: "hiddenFileName");

            var fileHidEvent = new FileHidEvent(
                initiatedBy: new DummyUserInfo(),
                id: hiddenFileId
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
            result.Single().FileName.Should().Be(expectedFileName);
        }

        private void AppendFileAddedEventToSession(Guid projectId, Guid fileId, string fileName)
        {
            var fileAddedEvent = new FileAddedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: projectId,
                id: fileId,
                fileName: fileName,
                location: new BlobLocation("container", "path")
                );

            Session.Events.Append(projectId, fileAddedEvent);
            Session.SaveChanges();
        }
    }
}
