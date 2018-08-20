using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.ClinicalNeeds;
using Xunit;

namespace Ubora.Domain.Tests.ClinicalNeeds
{
    public class ClinicalNeedQuickInfoViewProjectionTests : IntegrationFixture
    {
        [Fact]
        public void Foo()
        {
            var clinicalNeedId = Guid.NewGuid();

            // Act
            new ClinicalNeedSeeder(this, clinicalNeedId)
                .IndicateTheClinicalNeed();

            var deletedCommentId = Guid.NewGuid();
            new DiscussionSeeder(this, discussionId: clinicalNeedId)
                .AddComment()
                .AddComment()
                .AddComment(deletedCommentId)
                .DeleteComment(deletedCommentId);

            new ProjectSeeder()
                .WithRelatedClinicalNeed(clinicalNeedId).Seed(this);

            // Assert
            var projection = Session.Query<ClinicalNeedQuickInfo>().Single();

            using (new AssertionScope())
            {
                projection.NumberOfRelatedProjects.Should().Be(1);
                projection.NumberOfComments.Should().Be(2);
            }
        }
    }
}