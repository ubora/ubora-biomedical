using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Ubora.Domain.Discussions.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates.Events;

namespace Ubora.Domain.Discussions
{
    public class Discussion : Entity<Discussion>
    {
        public Guid Id { get; private set; }
        public virtual ImmutableList<Comment> Comments { get; private set; } = ImmutableList.Create<Comment>();
        public AttachedToEntity AttachedToEntity { get; private set; }
        public virtual ImmutableDictionary<string, object> AdditionalDiscussionData { get; private set; }

        private void Apply(DiscussionOpenedEvent e)
        {
            Id = e.DiscussionId;
            AttachedToEntity = e.AttachedToEntity;
            AdditionalDiscussionData = e.AdditionalDiscussionData;
        }
        
        private void Apply(CommentAddedEvent e)
        {
            var comment = Comment.Create(
                id: e.CommentId, 
                userId: e.InitiatedBy.UserId, 
                text: e.CommentText, 
                commentedAt: e.Timestamp,
                additionalData: e.AdditionalCommentData);

            Comments = Comments.Add(comment);
        }

        private void Apply(CommentEditedEvent e)
        {
            var oldComment = Comments.Single(x => x.Id == e.CommentId);

            var editedComment = oldComment.Edit(
                text: e.CommentText, 
                editedAt: e.Timestamp,
                additionalData: e.AdditionalCommentData);

            Comments = Comments.Replace(oldComment, editedComment);
        }

        private void Apply(CommentDeletedEvent e)
        {
            var comment = Comments.Single(x => x.Id == e.CommentId);
            Comments = Comments.Remove(comment);
        }

        #region Candidate legacy

        private void Apply(CandidateAddedEvent e)
        {
            Apply(new DiscussionOpenedEvent(
                initiatedBy: e.InitiatedBy,
                discussionId: e.Id,
                attachedToEntity: new AttachedToEntity(EntityName.Candidate, e.Id),
                additionalDiscussionData: new Dictionary<string, object>().ToImmutableDictionary()));
        }

        private void Apply(CandidateCommentAddedEvent e)
        {
            Apply(new CommentAddedEvent(
                initiatedBy: e.InitiatedBy,
                commentId: e.CommentId,
                commentText: e.CommentText,
                projectId: e.ProjectId,
                additionalCommentData: new Dictionary<string, object> { {"RoleKeys", e.RoleKeys} }.ToImmutableDictionary()));
        }

        private void Apply(CandidateCommentEditedEvent e)
        {
            Apply(new CommentEditedEvent(
                initiatedBy: e.InitiatedBy,
                commentId: e.CommentId,
                commentText: e.CommentText,
                projectId: e.ProjectId,
                additionalCommentData: new Dictionary<string, object> { {"RoleKeys", e.RoleKeys} }.ToImmutableDictionary()));
        }

        private void Apply(CandidateCommentRemovedEvent e)
        {
            Apply(new CommentDeletedEvent(
                initiatedBy: e.InitiatedBy,
                commentId: e.CommentId,
                projectId: e.ProjectId));
        }
        
        #endregion
    }
}