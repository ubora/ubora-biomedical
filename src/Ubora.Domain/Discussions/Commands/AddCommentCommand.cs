using System;
using System.Collections.Immutable;
using Marten;
using Ubora.Domain.Discussions.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Discussions.Commands
{
    /// <remarks>
    /// Leave ProjectId empty if not for project.
    /// </remarks>
    public class AddCommentCommand : UserProjectCommand
    {
        public Guid DiscussionId { get; set; }
        public Guid CommentId { get; set; } = Guid.NewGuid();
        public string CommentText { get; set; }
        public ImmutableDictionary<string, object> AdditionalCommentData { get; set; }

        internal class Handler : ICommandHandler<AddCommentCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AddCommentCommand cmd)
            {
                var @event = new CommentAddedEvent(
                    initiatedBy: cmd.Actor,
                    commentId: cmd.CommentId,
                    commentText: cmd.CommentText,
                    projectId: cmd.ProjectId,
                    additionalCommentData: cmd.AdditionalCommentData);

                _documentSession.Events.Append(cmd.DiscussionId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}