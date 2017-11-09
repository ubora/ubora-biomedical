using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class CreateProjectCommand : UserCommand
    {
        public Guid NewProjectId { get; set; }
        public string Title { get; set; }
        public string ClinicalNeed { get; set; }
        public string AreaOfUsage { get; set; }
        public string PotentialTechnology { get; set; }
        public string Gmdn { get; set; }

        internal class Handler : CommandHandler<CreateProjectCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(CreateProjectCommand cmd)
            {
                var @event = new ProjectCreatedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.NewProjectId,
                    title: cmd.Title,
                    clinicalNeed: cmd.ClinicalNeed,
                    areaOfUsage: cmd.AreaOfUsage,
                    potentialTechnology: cmd.PotentialTechnology,
                    gmdn: cmd.Gmdn);

                DocumentSession.Events.Append(cmd.NewProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}