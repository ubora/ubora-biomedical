using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Events;
using Ubora.Domain.Projects.Candidates.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Candidates.Specifications
{
    public class IsProjectCandidateSpecTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_Candidates_Which_Are_From_Project()
        {
            var expectedProjectId = Guid.NewGuid();
            new ProjectSeeder().WithId(expectedProjectId).Seed(this);
            var expectedCandidateId = Guid.NewGuid();
            var userInfo = new DummyUserInfo();
            var candidateAddedEvent = new CandidateAddedEvent(
                initiatedBy: userInfo,
                projectId: expectedProjectId,
                id: expectedCandidateId,
                title: "title",
                description: "description",
                imageLocation: new BlobLocation("container", "path"));

            Session.Events.Append(expectedProjectId, candidateAddedEvent);
            Session.SaveChanges();

            var otherProjectId = Guid.NewGuid();
            new ProjectSeeder().WithId(otherProjectId).Seed(this);
            var otherCandidateAddedEvent = new CandidateAddedEvent(
                initiatedBy: userInfo,
                projectId: otherProjectId,
                id: Guid.NewGuid(),
                title: "title",
                description: "description",
                imageLocation: new BlobLocation("container", "path"));

            Session.Events.Append(otherProjectId, otherCandidateAddedEvent);
            Session.SaveChanges();

            var sut = new IsProjectCandidateSpec(expectedProjectId);

            RefreshSession();

            var candidatesQueryable = Session.Query<Candidate>();

            // Act
            var result = sut.SatisfyEntitiesFrom(candidatesQueryable);

            // Assert
            var candidate = result.Single();
            candidate.Id.Should().Be(expectedCandidateId);
        }
    }
}
