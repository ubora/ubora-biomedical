using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Repository.Commands;
using Ubora.Domain.Projects.Repository.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Repository.Commands
{
    public class UpdateFileCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _fileId = Guid.NewGuid();
        private readonly string _fileName = "fileName";
        private readonly string _folderName = "folderName";
        private readonly BlobLocation _blobLocation = new BlobLocation("expectedContainer", "expectedBlobPath");
        private readonly string _comment = "comment";
        private readonly long _fileSize = 121323421342;

        [Fact]
        public void Updates_File_From_Project()
        {
            this.Given(_ => Add_File_To_Project())
                .When(_ => User_Updates_File_From_Project())
                .Then(_ => Assert_ProjectFile_Is_Updated_In_Project())
                .Then(_ => Assert_FileUpdated_Is_Added_In_Events())
                .BDDfy();
        }

        private void Add_File_To_Project()
        {
            var fileAddedEvent = new FileAddedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: _projectId,
                id: _fileId,
                fileName: _fileName,
                comment: _comment,
                fileSize: _fileSize,
                folderName: _folderName,
                location: new BlobLocation("container", "path"));
            Session.Events.Append(_projectId, fileAddedEvent);
            Session.SaveChanges();
        }

        private void User_Updates_File_From_Project()
        {
            var updateFileCommand = new UpdateFileCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = _projectId,
                Id = _fileId,
                BlobLocation = _blobLocation
            };

            Processor.Execute(updateFileCommand);
        }

        private void Assert_ProjectFile_Is_Updated_In_Project()
        {
            var file = Session.Load<ProjectFile>(_fileId);

            file.Id.Should().Be(_fileId);
            file.ProjectId.Should().Be(_projectId);
            file.FileName.Should().Be(_fileName);
            file.FolderName.Should().Be(_folderName);
            file.Location.Should().Be(_blobLocation);
            file.RevisionNumber.Should().Be(2);
            file.IsHidden.Should().BeFalse();
        }

        private void Assert_FileUpdated_Is_Added_In_Events()
        {
            var fileUpdateEvents = Session.Events.QueryRawEventDataOnly<FileUpdatedEvent>();

            fileUpdateEvents.Count().Should().Be(1);
            fileUpdateEvents.First().Id.Should().Be(_fileId);
            fileUpdateEvents.First().ProjectId.Should().Be(_projectId);
            fileUpdateEvents.First().Location.BlobPath.Should().Be(_blobLocation.BlobPath);
        }
    }
}
