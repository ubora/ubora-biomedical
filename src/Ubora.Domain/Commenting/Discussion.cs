using System;
using System.Collections.Immutable;
using System.Linq;
using Ubora.Domain.Commenting.Events;

namespace Ubora.Domain.Commenting
{
    public class Discussion
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public ImmutableList<Comment> Comments { get; set; } = ImmutableList.Create<Comment>();
        public DiscussionAttachedToEntity AttachedTo { get; set; }

        private void Apply(CommentAddedEvent e)
        {
            var comment = Comment.Create(
                id: e.CommentId, 
                userId: e.InitiatedBy.UserId, 
                text: e.CommentText, 
                commentedAt: e.Timestamp.DateTime);

            Comments = Comments.Add(comment);
        }

        private void Apply(CommentEditedEvent e)
        {
            var oldComment = Comments.Single(x => x.Id == e.CommentId);

            var editedComment = oldComment.Edit(
                text: e.CommentText, 
                editedAt: e.Timestamp.DateTime);

            Comments = Comments.Replace(oldComment, editedComment);
        }

        private void Apply(CommentDeletedEvent e)
        {
            var comment = Comments.Single(x => x.Id == e.CommentId);
            Comments = Comments.Remove(comment);
        }
    }
}