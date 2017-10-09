using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Repository.Events;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects._Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Specifications
{
    public class IsHiddenFileSpecTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_ProjectFiles_Which_Are_Hidden()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var userInfo = new DummyUserInfo();
            var expectedFileId = Guid.NewGuid();
            var fileAddedEvent = new FileAddedEvent(
                initiatedBy: userInfo,
                projectId: projectId,
                id: expectedFileId,
                fileName: "expectedFileName",
                folderName: "folderName",
                comment: "comment",
                fileSize: 12131,
                location: new BlobLocation("container","path"),
                revisionNumber: 1);
            Session.Events.Append(projectId, fileAddedEvent);
            Session.SaveChanges();

            var otherFileAddedEvent = new FileAddedEvent(
                initiatedBy: userInfo,
                projectId: projectId,
                id: Guid.NewGuid(),
                fileName: "fileName",
                folderName: "folderName",
                comment: "comment",
                fileSize: 12344,
                location: new BlobLocation("container", "path"),
                revisionNumber: 1);
            Session.Events.Append(projectId, otherFileAddedEvent);
            Session.SaveChanges();

            var fileHiddenEvent = new FileHiddenEvent(
                initiatedBy: userInfo,
                id: expectedFileId,
                projectId:projectId
                );
            Session.Events.Append(projectId, fileHiddenEvent);
            Session.SaveChanges();

            var sut = new IsHiddenFileSpec();

            RefreshSession();

            var projectFilesQueryable = Session.Query<ProjectFile>();

            // Act
            var result = sut.SatisfyEntitiesFrom(projectFilesQueryable);

            // Assert
            var projectFile = result.Single();
            projectFile.Id.Should().Be(expectedFileId);
        }
    }
}
