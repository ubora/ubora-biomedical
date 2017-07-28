using System;
using System.Linq;
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
            var expectedBlobPath = "blobPath";
            var expectedFileName = "expectedFileName";
            var command = new AddFileCommand
            {
                FileName = expectedFileName,
                ProjectId = expectedProjectId,
                Id = expectedFileId,
                Actor = new DummyUserInfo(),
                BlobLocation = new BlobLocation("container", expectedBlobPath)
            };

            // Act
            Processor.Execute(command);

            // Assert
            var file = Session.Load<ProjectFile>(expectedFileId);

            file.Id.Should().Be(expectedFileId);
            file.ProjectId.Should().Be(expectedProjectId);
            file.FileName.Should().Be(expectedFileName);
            file.Location.BlobPath.Should().Be(expectedBlobPath);
            file.IsHidden.Should().BeFalse();

            var fileAddedEvents = Session.Events.QueryRawEventDataOnly<FileAddedEvent>();

            fileAddedEvents.Count().Should().Be(1);
            fileAddedEvents.First().Id.Should().Be(expectedFileId);
            fileAddedEvents.First().FileName.Should().Be(command.FileName);
            fileAddedEvents.First().Location.BlobPath.Should().Be(expectedBlobPath);
            fileAddedEvents.First().ProjectId.Should().Be(expectedProjectId);
        }
    }
}
