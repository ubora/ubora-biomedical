using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates.Events;

namespace Ubora.Domain.Projects.Candidates.Commands
{
    public class AddCandidateVoteCommand : UserProjectCommand
    {
        public Guid CandidateId { get; set; }
        public int Functionality { get; set; }
        public int Performance { get; set; }
        public int Usability { get; set; }
        public int Safety { get; set; }

        internal class Handler : CommandHandler<AddCandidateVoteCommand>
        {

            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AddCandidateVoteCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var candidate = DocumentSession.LoadOrThrow<Candidate>(cmd.CandidateId);

                var @event = new CandidateVoteAddedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    candidateId: cmd.CandidateId,
                    functionality: cmd.Functionality,
                    perfomance: cmd.Performance,
                    usability: cmd.Usability,
                    safety: cmd.Safety
                );

                DocumentSession.Events.Append(cmd.CandidateId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
