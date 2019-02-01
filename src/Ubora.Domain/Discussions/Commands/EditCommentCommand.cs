using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Marten;
using Ubora.Domain.Discussions.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Discussions.Commands
{
    /// <remarks>
    /// Leave ProjectId empty if not for project.
    /// </remarks>
    public class EditCommentCommand : UserProjectCommand
    {
        public Guid DiscussionId { get; set; }
        public Guid CommentId { get; set; }
        public string CommentText { get; set; }
        public ImmutableDictionary<string, object> AdditionalCommentData { get; set; }

        internal class Handler : ICommandHandler<EditCommentCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(EditCommentCommand cmd)
            {
                _documentSession.LoadOrThrow<Discussion>(cmd.DiscussionId);

                var @event = new CommentEditedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    commentId: cmd.CommentId,
                    commentText: cmd.CommentText,
                    additionalCommentData: cmd.AdditionalCommentData
                );

                _documentSession.Events.Append(cmd.DiscussionId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}