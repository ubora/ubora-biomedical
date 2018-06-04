using System;
using System.Collections.Generic;
using Marten;
using Ubora.Domain.Commenting.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Commenting.Commands
{
    public class EditCommentCommand : UserCommand
    {
        public Guid DiscussionId { get; set; }
        public Guid CommentId { get; set; }
        public string CommentText { get; set; }
        public Guid? ProjectId { get; set; }

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
                    additional: new Dictionary<string, object>()
                );

                _documentSession.Events.Append(cmd.DiscussionId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}