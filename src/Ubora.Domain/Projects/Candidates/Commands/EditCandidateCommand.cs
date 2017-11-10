using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates.Events;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Projects.Candidates.Commands
{
    public class EditCandidateCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        internal class Handler : CommandHandler<EditCandidateCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditCandidateCommand cmd)
            {
                var workpackageTwo = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var @event = new CandidateEditedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    id: cmd.Id,
                    title: cmd.Title,
                    description: cmd.Description
                );

                DocumentSession.Events.Append(cmd.Id, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
