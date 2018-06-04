using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Commenting
{
    public class Comment
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Text { get; }
        public DateTime CommentedAt { get; }
        public DateTime LastEditedAt { get; }

        [JsonConstructor]
        private Comment(Guid id, Guid userId, string text, DateTime commentedAt, DateTime lastEditedAt)
        {
            Id = id;
            UserId = userId;
            Text = text;
            CommentedAt = commentedAt;
            LastEditedAt = lastEditedAt;
        }

        public static Comment Create(Guid id, Guid userId, string text, DateTime commentedAt)
        {
            return new Comment(
                id: id,
                userId: userId,
                text: text,
                commentedAt: commentedAt,
                lastEditedAt: default(DateTime));
        }

        public Comment Edit(string text, DateTime editedAt)
        {
            return new Comment(
                id: Id,
                userId: UserId,
                text: text,
                commentedAt: CommentedAt,
                lastEditedAt: editedAt);
        }
    }
}