using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates.Events;

namespace Ubora.Domain.Projects.Candidates.Commands
{
    public class RemoveCandidateCommand : UserProjectCommand
    {
        public Guid CandidateId { get; set; }

        internal class Handler : CommandHandler<RemoveCandidateCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(RemoveCandidateCommand cmd)
            {
                var candidate = DocumentSession.LoadOrThrow<Candidate>(cmd.CandidateId);

                var @event = new CandidateRemovedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId
                );

                DocumentSession.Events.Append(candidate.Id, @event);
                DocumentSession.Delete<Candidate>(candidate.Id);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
