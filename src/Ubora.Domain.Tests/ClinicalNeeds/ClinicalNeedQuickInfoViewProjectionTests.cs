using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.Discussions;
using Xunit;

namespace Ubora.Domain.Tests.ClinicalNeeds
{
    public class ClinicalNeedQuickInfoViewProjectionTests : IntegrationFixture
    {
        [Fact]
        public void NumberOfComments_And_NumberOfRelatedProjects_Are_Counted_Properly()
        {
            var clinicalNeedId = Guid.NewGuid();

            // Act
            new ClinicalNeedSeeder(this, clinicalNeedId)
                .IndicateTheClinicalNeed();

            var deletedCommentId = Guid.NewGuid();
            new DiscussionSeeder(this, discussionId: clinicalNeedId)
                .AddComment()
                .AddComment(deletedCommentId)
                .DeleteComment(deletedCommentId)
                .AddComment();

            new ProjectSeeder()
                .WithRelatedClinicalNeed(clinicalNeedId)
                .Seed(this);

            new ProjectSeeder()
                .WithRelatedClinicalNeed(clinicalNeedId)
                .Seed(this);

            new ProjectSeeder()
                .WithRelatedClinicalNeed(clinicalNeedId)
                .AsDeleted()
                .Seed(this);

            // Assert
            var projection = Session.Query<ClinicalNeedQuickInfo>().Single();

            using (new AssertionScope())
            {
                projection.NumberOfRelatedProjects.Should().Be(2);
                projection.NumberOfComments.Should().Be(2);
            }
        }

        [Fact]
        public void LastActivityAt_Is_Marked_Properly()
        {
            var clinicalNeedId = Guid.NewGuid();

            // Act & Assert 1
            new ClinicalNeedSeeder(this, clinicalNeedId)
                .IndicateTheClinicalNeed();

            var clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);
            var quickInfo = Session.Load<ClinicalNeedQuickInfo>(clinicalNeedId);

            quickInfo.LastActivityAt.Should().Be(clinicalNeed.IndicatedAt);

            // Act & Assert 2
            var discussionSeeder = 
                new DiscussionSeeder(this, discussionId: clinicalNeedId)
                    .AddComment();

            var discussion = Session.Load<Discussion>(clinicalNeedId);
            quickInfo = Session.Load<ClinicalNeedQuickInfo>(clinicalNeedId);

            quickInfo.LastActivityAt.Should().Be(discussion.LastCommentActivityAt.Value);

            // Act & Assert 3
            var deletedCommentId = Guid.NewGuid();
            discussionSeeder
                .AddComment(deletedCommentId)
                .DeleteComment(deletedCommentId);

            discussion = Session.Load<Discussion>(clinicalNeedId);
            quickInfo = Session.Load<ClinicalNeedQuickInfo>(clinicalNeedId);

            quickInfo.LastActivityAt.Should().Be(discussion.LastCommentActivityAt.Value);

            // Act & Assert 4
            var editedCommentId = Guid.NewGuid();
            discussionSeeder
                .AddComment(editedCommentId)
                .EditComment(editedCommentId);

            discussion = Session.Load<Discussion>(clinicalNeedId);
            quickInfo = Session.Load<ClinicalNeedQuickInfo>(clinicalNeedId);

            quickInfo.LastActivityAt.Should().Be(discussion.LastCommentActivityAt.Value);
        }
    }
}