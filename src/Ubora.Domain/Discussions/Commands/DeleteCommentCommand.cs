using System;
using Marten;
using Ubora.Domain.Discussions.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Discussions.Commands
{
    /// <remarks>
    /// Leave ProjectId empty if not for project.
    /// </remarks>
    public class DeleteCommentCommand : UserProjectCommand
    {
        public Guid DiscussionId { get; set; }
        public Guid CommentId { get; set; }

        internal class Handler : ICommandHandler<DeleteCommentCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeleteCommentCommand cmd)
            {
                var @event = new CommentDeletedEvent(
                    initiatedBy: cmd.Actor,
                    commentId: cmd.CommentId,
                    projectId: cmd.ProjectId);

                _documentSession.Events.Append(cmd.DiscussionId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}