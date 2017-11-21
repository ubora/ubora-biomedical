using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Projects._Commands
{
    public class DeleteCandidateImageCommand : UserProjectCommand
    {
        public Guid CandidateId { get; set; }
        internal class Handler : CommandHandler<DeleteCandidateImageCommand>
        {

            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(DeleteCandidateImageCommand cmd)
            {
                var workpackageTwo = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var @event = new CandidateImageDeletedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    candidateId: cmd.CandidateId,
                    when: DateTime.UtcNow);

                DocumentSession.Events.Append(cmd.CandidateId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
