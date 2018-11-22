using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Ubora.Domain.Discussions
{
    public class Comment
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Text { get; }
        public DateTimeOffset CommentedAt { get; }
        public DateTimeOffset? LastEditedAt { get; }
        public ImmutableDictionary<string, object> AdditionalData { get; }

        [JsonConstructor]
        private Comment(Guid id, Guid userId, string text, DateTimeOffset commentedAt, DateTimeOffset? lastEditedAt, ImmutableDictionary<string, object> additionalData)
        {
            Id = id;
            UserId = userId;
            Text = text;
            CommentedAt = commentedAt;
            LastEditedAt = lastEditedAt;
            AdditionalData = additionalData;
        }

        public static Comment Create(Guid id, Guid userId, string text, DateTimeOffset commentedAt, ImmutableDictionary<string, object> additionalData)
        {
            return new Comment(
                id: id,
                userId: userId,
                text: text,
                commentedAt: commentedAt,
                lastEditedAt: null,
                additionalData: additionalData);
        }

        public Comment Edit(string text, DateTimeOffset editedAt, ImmutableDictionary<string, object> additionalData)
        {
            return new Comment(
                id: Id,
                userId: UserId,
                text: text,
                commentedAt: CommentedAt,
                lastEditedAt: editedAt,
                additionalData: additionalData);
        }
    }
}