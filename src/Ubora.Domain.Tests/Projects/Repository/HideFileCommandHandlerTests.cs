using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Repository
{
    public class HideFileCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _fileId = Guid.NewGuid();
        private readonly string _fileName = "expectedFileName";
        private readonly BlobLocation _blobLocation = new BlobLocation("container", "blobPath");

        [Fact]
        public void Hides_File_From_Project()
        {
            this.Given(_ => Add_File_To_Project())
                .When(_ => User_Hides_File_From_Project())
                .Then(_ => Assert_ProjectFile_Is_Hid_In_Project())
                .Then(_ => Assert_FileHid_Is_Added_In_Events())
                .BDDfy();
        }

        private void Add_File_To_Project()
        {
            var fileAddedEvent = new FileAddedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: _projectId,
                id: _fileId,
                fileName: _fileName,
                location: _blobLocation);
            Session.Events.Append(_projectId, fileAddedEvent);
            Session.SaveChanges();
        }

        private void User_Hides_File_From_Project()
        {
            var command = new HideFileCommand
            {
                Id = _fileId,
                Actor = new DummyUserInfo()
            };

            Processor.Execute(command);
        }

        private void Assert_ProjectFile_Is_Hid_In_Project()
        {
            var file = Session.Load<ProjectFile>(_fileId);

            file.Id.Should().Be(_fileId);
            file.ProjectId.Should().Be(_projectId);
            file.FileName.Should().Be(_fileName);
            file.Location.Should().Be(_blobLocation);
            file.IsHidden.Should().BeTrue();
        }

        private void Assert_FileHid_Is_Added_In_Events()
        {
            var fileHidEvents = Session.Events.QueryRawEventDataOnly<FileHiddenEvent>();

            fileHidEvents.Count().Should().Be(1);
            fileHidEvents.First().Id.Should().Be(_fileId);
        }
    }
}
