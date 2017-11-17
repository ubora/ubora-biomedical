using System;

namespace Ubora.Domain.Projects.Candidates
{
    public class Comment
    {
        public Comment(Guid userId, string text)
        {
            UserId = userId;
            Text = text;
        }

        public Guid UserId { get; private set; }
        public string Text { get; private set; }
    }
}
