using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Repository.Commands;
using Ubora.Domain.Projects.Repository.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Repository
{
    public class AddFileCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _fileId = Guid.NewGuid();
        private readonly string _fileName = "fileName";
        private readonly string _folderName = "folderName";
        private readonly BlobLocation _blobLocation = new BlobLocation("expectedContainer", "expectedBlobPath");

        [Fact]
        public void Adds_New_File_To_Project()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => Add_File_To_Project())
                .Then(_ => Assert_ProjectFile_Is_Added_In_Project())
                .Then(_ => Assert_FileAdded_Is_Added_In_Events())
                .BDDfy();
        }

        private void Add_File_To_Project()
        {
            var command = new AddFileCommand
            {
                FileName = _fileName,
                ProjectId = _projectId,
                Id = _fileId,
                Actor = new DummyUserInfo(),
                BlobLocation = _blobLocation,
                FolderName = _folderName
            };

            // Act
            Processor.Execute(command);
        }

        private void Assert_ProjectFile_Is_Added_In_Project()
        {
            var file = Session.Load<ProjectFile>(_fileId);

            file.Id.Should().Be(_fileId);
            file.ProjectId.Should().Be(_projectId);
            file.FileName.Should().Be(_fileName);
            file.FolderName.Should().Be(_folderName);
            file.Location.Should().Be(_blobLocation);
            file.RevisionNumber.Should().Be(1);
            file.IsHidden.Should().BeFalse();
        }

        private void Assert_FileAdded_Is_Added_In_Events()
        {
            var fileAddedEvents = Session.Events.QueryRawEventDataOnly<FileAddedEvent>();

            fileAddedEvents.Count().Should().Be(1);
            fileAddedEvents.First().Id.Should().Be(_fileId);
            fileAddedEvents.First().ProjectId.Should().Be(_projectId);
            fileAddedEvents.First().Location.BlobPath.Should().Be(_blobLocation.BlobPath);
        }
    }
}
