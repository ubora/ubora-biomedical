using Marten;
using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates.Events;

namespace Ubora.Domain.Projects.Candidates.Commands
{
    public class EditCandidateCommentCommand : UserProjectCommand
    {
        public Guid CommentId { get; set; }
        public Guid CandidateId { get; set; }
        public string CommentText { get; set; }

        internal class Handler : CommandHandler<EditCandidateCommentCommand>
        {

            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditCandidateCommentCommand cmd)
            {
                var candidate = DocumentSession.LoadOrThrow<Candidate>(cmd.CandidateId);

                var @event = new CandidateCommentEditedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    commentId: cmd.CommentId,
                    commentText: cmd.CommentText,
                    lastEditedAt: DateTime.UtcNow
                );

                DocumentSession.Events.Append(cmd.CandidateId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
