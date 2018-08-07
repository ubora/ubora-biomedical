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

            var command = new AddCommentCommand
            {
                Actor = new DummyUserInfo(),
                CommentText = "testCommentText",
                DiscussionId = discussionCreatedEvent.DiscussionId,
                AdditionalCommentData = new Dictionary<string, object> { { "test", 123 } }.ToImmutableDictionary(),
                ProjectId = default(Guid)
            };

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var streamEvents = Session.Events.FetchStream(discussionId);
            streamEvents.Should().HaveCount(2);

            var persistedEvent = (CommentAddedEvent)streamEvents.Select(martenEvent => martenEvent.Data).Last();
            persistedEvent.InitiatedBy.ShouldBeEquivalentTo(command.Actor);
            persistedEvent.CommentId.Should().NotBe(default(Guid));
            persistedEvent.ProjectId.Should().Be(command.ProjectId);
            persistedEvent.AdditionalCommentData.ShouldBeEquivalentTo(command.AdditionalCommentData);
        }
    }
}
