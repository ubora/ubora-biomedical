using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Ubora.Domain.Discussions;
using Ubora.Domain.Discussions.Commands;
using Ubora.Domain.Discussions.Events;
using Xunit;

namespace Ubora.Domain.Tests.Discussions.Commands
{
    public class AddCommentCommandTests : IntegrationFixture
    {
        [Fact]
        public void Comment_Can_Be_Added()
        {
            // Create discussion
            var discussionId = Guid.NewGuid();
            var discussionCreatedEvent = new DiscussionOpenedEvent(
                initiatedBy: new DummyUserInfo(),
                discussionId: discussionId,
                attachedToEntity: new AttachedToEntity(EntityName.Candidate, Guid.NewGuid()),
                additionalDiscussionData: new Dictionary<string, object>().ToImmutableDictionary());
            Session.Events.Append(discussionCreatedEvent.DiscussionId, discussionCreatedEvent);
            //

            var commentId = Guid.NewGuid();
            var actor = new DummyUserInfo();
            var command = new AddCommentCommand
            {
                Actor = actor,
                CommentText = "testCommentText",
                DiscussionId = discussionCreatedEvent.DiscussionId,
                AdditionalCommentData = new Dictionary<string, object> { { "test", 123 } }.ToImmutableDictionary(),
                ProjectId = default(Guid),
                CommentId = commentId
            };

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var streamEvents = Session.Events.FetchStream(discussionId);
            streamEvents.Should().HaveCount(2);

            var persistedEvent = (CommentAddedEvent)streamEvents.Select(martenEvent => martenEvent.Data).Last();
            persistedEvent.InitiatedBy.ShouldBeEquivalentTo(actor);
            persistedEvent.CommentId.Should().Be(commentId);
            persistedEvent.ProjectId.Should().Be(command.ProjectId);
            persistedEvent.AdditionalCommentData.ShouldBeEquivalentTo(command.AdditionalCommentData);

            var comment = Session.Load<Discussion>(discussionId).Comments.Single();
            comment.Id.Should().Be(commentId);
            comment.Text.Should().Be("testCommentText");
            comment.LastEditedAt.Should().BeNull();
            comment.AdditionalData.ShouldBeEquivalentTo(command.AdditionalCommentData);
            comment.UserId.Should().Be(actor.UserId);
        }
    }
}
