﻿using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Repository.Events;
using Ubora.Domain.Projects._Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Specifications
{
    public class IsProjectFileSpecTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_ProjectFiles_Which_Are_From_Project()
        {
            var expectedProjectId = Guid.NewGuid();
            var expectedFileId = Guid.NewGuid();
            var userInfo = new DummyUserInfo();
            var fileAddedEvent = new FileAddedEvent(
                initiatedBy: userInfo,
                projectId: expectedProjectId,
                id: expectedFileId,
                fileName: "expectedFileName",
                folderName: "folderName",
                comment: "comment",
                fileSize: 1234,
                location: new BlobLocation("container", "path"),
                revisionNumber: 1);
            Session.Events.Append(expectedProjectId, fileAddedEvent);
            Session.SaveChanges();

            var otherProjectId = Guid.NewGuid();
            var otherFileAddedEvent = new FileAddedEvent(
                initiatedBy: userInfo,
                projectId: otherProjectId,
                id: Guid.NewGuid(),
                fileName: "fileName",
                folderName: "folderName",
                comment: "comment",
                fileSize: 1234,
                location: new BlobLocation("container", "path"),
                revisionNumber: 1);
            Session.Events.Append(otherProjectId, otherFileAddedEvent);
            Session.SaveChanges();

            var sut = new IsProjectFileSpec(expectedProjectId);

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
