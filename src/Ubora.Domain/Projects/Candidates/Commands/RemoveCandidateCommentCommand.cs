using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates.Events;

namespace Ubora.Domain.Projects.Candidates.Commands
{
    public class RemoveCandidateCommentCommand : UserProjectCommand
    {
        public Guid CommentId { get; set; }
        public Guid CandidateId { get; set; }

        internal class Handler : CommandHandler<RemoveCandidateCommentCommand>
        {

            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(RemoveCandidateCommentCommand cmd)
            {
                var candidate = DocumentSession.LoadOrThrow<Candidate>(cmd.CandidateId);

                var @event = new CandidateCommentRemovedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    commentId: cmd.CommentId
                );

                DocumentSession.Events.Append(cmd.CandidateId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }

    }
}
