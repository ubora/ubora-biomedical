using System;

namespace Ubora.Domain.Projects.Candidates
{
    public class Comment
    {
        public Comment(Guid userId, string text, Guid commentId, DateTime commentedAt, string[] roleKeys)
        {
            UserId = userId;
            Text = text;
            Id = commentId;
            CommentedAt = commentedAt;
            RoleKeys = roleKeys;
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Text { get; private set; }
        public DateTime CommentedAt { get; private set; }
        public DateTime LastEditedAt { get; private set; }
        public string[] RoleKeys { get; private set; }

        public Comment Edit(string text, DateTime lastEditedAt, string[] roleKeys)
        {
            return new Comment(this.UserId, text, this.Id, this.CommentedAt, roleKeys)
            {
                LastEditedAt = lastEditedAt
            };
        }
    }
}
