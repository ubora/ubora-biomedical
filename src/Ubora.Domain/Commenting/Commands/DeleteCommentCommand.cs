using System;
using System.Collections.Generic;
using Marten;
using Ubora.Domain.Commenting.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Commenting.Commands
{
    public class DeleteCommentCommand : UserCommand
    {
        public Guid DiscussionId { get; set; }
        public Guid CommentId { get; set; }
        public Guid? ProjectId { get; set; }

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
                    projectId: cmd.ProjectId,
                    additional: new Dictionary<string, object>());

                _documentSession.Events.Append(cmd.DiscussionId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}