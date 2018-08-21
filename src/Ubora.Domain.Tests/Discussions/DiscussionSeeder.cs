using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Ubora.Domain.Discussions.Commands;

namespace Ubora.Domain.Tests.ClinicalNeeds
{
    public class DiscussionSeeder
    {
        private readonly IntegrationFixture _fixture;

        public DiscussionSeeder(IntegrationFixture fixture, Guid? discussionId = null)
        {
            _fixture = fixture;
            DiscussionId = discussionId ?? Guid.NewGuid();
        }

        public Guid DiscussionId { get; set; }

        public DiscussionSeeder AddComment(Guid? commentId = null)
        {
            commentId = commentId ?? Guid.NewGuid();

            var command = new AddCommentCommand
            {
                DiscussionId = DiscussionId,
                CommentId = commentId.Value,
                CommentText = Guid.NewGuid().ToString(),
                AdditionalCommentData = new Dictionary<string, object>().ToImmutableDictionary(),
                Actor = new DummyUserInfo()
            };

            var commandResult = _fixture.Processor.Execute(command);
            if (commandResult.IsFailure)
            {
                throw new Exception(commandResult.ToString());
            }
            return this;
        }

        public DiscussionSeeder DeleteComment(Guid commentId)
        {
            var command = new DeleteCommentCommand
            {
                DiscussionId = DiscussionId,
                CommentId = commentId,
                Actor = new DummyUserInfo()
            };

            var commandResult = _fixture.Processor.Execute(command);
            if (commandResult.IsFailure)
            {
                throw new Exception(commandResult.ToString());
            }
            return this;
        }

        public DiscussionSeeder EditComment(Guid commentId)
        {
            var command = new EditCommentCommand
            {
                DiscussionId = DiscussionId,
                CommentId = commentId,
                CommentText = Guid.NewGuid().ToString(),
                AdditionalCommentData = new Dictionary<string, object>().ToImmutableDictionary(),
                Actor = new DummyUserInfo()
            };

            var commandResult = _fixture.Processor.Execute(command);
            if (commandResult.IsFailure)
            {
                throw new Exception(commandResult.ToString());
            }
            return this;
        }
    }
}