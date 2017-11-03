using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class DeleteCandidateImageCommand : UserProjectCommand

    {
        public Guid CandidateId { get; set; }
        internal class Handler : ICommandHandler<DeleteCandidateImageCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeleteCandidateImageCommand cmd)
            {
                var candidate = _documentSession.LoadOrThrow<Candidate>(cmd.CandidateId);

                var @event = new CandidateImageDeletedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    candidateId: cmd.CandidateId,
                    when: DateTime.UtcNow);

                _documentSession.Events.Append(candidate.Id, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
