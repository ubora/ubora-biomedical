using System;
using System.Collections.Generic;
using Marten;
using Ubora.Domain.Commenting.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Commenting.Commands
{
    public class AddCommentCommand : UserCommand
    {
        public Guid DiscussionId { get; set; }
        public string CommentText { get; set; }
        public Guid? ProjectId { get; set; }

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
                    commentId: Guid.NewGuid(),
                    commentText: cmd.CommentText,
                    projectId: cmd.ProjectId,
                    additional: new Dictionary<string, object>());

                _documentSession.Events.Append(cmd.DiscussionId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}