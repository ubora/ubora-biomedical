using Marten;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates.Events;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Projects.Candidates.Commands
{
    public class AddCandidateCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BlobLocation ImageLocation { get; set; }

        internal class Handler : CommandHandler<AddCandidateCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AddCandidateCommand cmd)
            {
                var workpackageTwo = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var @event = new CandidateAddedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    id: cmd.Id,
                    title: cmd.Title,
                    description: cmd.Description,
                    imageLocation: cmd.ImageLocation
                );

                DocumentSession.Events.StartStream(cmd.Id, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
