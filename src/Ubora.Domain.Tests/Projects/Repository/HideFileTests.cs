using System;
using System.Linq;
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
            var fileName = "expectedFileName";
            Processor.Execute(new AddFileCommand
            {
                FileName = fileName,
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
            file.FileName.Should().Be(fileName);
            file.Location.BlobPath.Should().Be("blobPath");
            file.IsHidden.Should().BeTrue();

            var fileHidEvents = Session.Events.QueryRawEventDataOnly<FileHidEvent>();

            fileHidEvents.Count().Should().Be(1);
            fileHidEvents.First().Id.Should().Be(expectedFileId);
            fileHidEvents.First().FileName.Should().Be(fileName);
        }
    }
}
