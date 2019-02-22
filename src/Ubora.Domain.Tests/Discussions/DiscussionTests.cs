using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Ubora.Domain.Discussions;
using Ubora.Domain.Discussions.Events;
using Xunit;

namespace Ubora.Domain.Tests.Discussions
{
    public class DiscussionTests
    {
        [Fact]
        public void Apply_DiscussionOpenedEvent_Maps_Properties()
        {
            var discussion = new Discussion();

            var @event = new DiscussionOpenedEvent(
                initiatedBy: new DummyUserInfo(),
                discussionId: Guid.NewGuid(),
                attachedToEntity: new AttachedToEntity(EntityName.Candidate, Guid.NewGuid()),
                additionalDiscussionData: new Dictionary<string, object>().ToImmutableDictionary());

            // Act
            discussion.Apply(@event);

            // Assert
            discussion.Id.Should().Be(@event.DiscussionId);
            discussion.AttachedToEntity.Should().BeSameAs(@event.AttachedToEntity);
            discussion.AdditionalDiscussionData.Should().BeSameAs(@event.AdditionalDiscussionData);
            discussion.Comments.Should().BeEmpty();
        }

        [Fact]
        public void Apply_CommentAddedEvent_Maps_Properties()
        {
            var discussion = new Discussion().Set(d => d.Id, Guid.NewGuid());

            var @event = new CommentAddedEvent(
                projectId: Guid.NewGuid(),
                initiatedBy: new DummyUserInfo(),
                commentId: Guid.NewGuid(),
                commentText: "testCommentText",
                additionalCommentData: new Dictionary<string, object>().ToImmutableDictionary());

            // Act
            discussion.Apply(@event);

            // Assert
            Comment comment = discussion.Comments.Single();

            comment.Id.Should().Be(@event.CommentId);
            comment.CommentedAt.Should().Be(@event.Timestamp);
            comment.Text.Should().Be("testCommentText");
            comment.UserId.Should().Be(@event.InitiatedBy.UserId);
            comment.LastEditedAt.Should().BeNull();
        }

        [Fact]
        public void Apply_CommentEditedEvent_Edits_Existing_Comment()
        {
            var editedCommentInitial = Comment.Create(id: Guid.NewGuid(), text: "initialText", userId: Guid.NewGuid(), commentedAt: DateTimeOffset.Now, additionalData: new Dictionary<string, object>().ToImmutableDictionary());
            var comments = new[]
            {
                TestComments.CreateDummy(),
                editedCommentInitial,
                TestComments.CreateDummy()
            }.ToImmutableList();

            var discussion = new Discussion().Set(d => d.Comments, comments);

            //var @event = new CommentEditedEvent(
            //    initiatedBy: new DummyUserInfo(),
            //    commentId: editedCommentInitial.Id,
            //    commentText: "editedText",
            //    additionalCommentData: new Dictionary<string, object>);

            //// Act
            //discussion.Apply();
        }
    }
}
