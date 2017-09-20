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
                var project = DocumentSession.LoadOrThrow<Project>(cmd.NewProjectId);

                var @event = new ProjectCreatedEvent(cmd.Actor)
                {
                    Id = cmd.NewProjectId,
                    Title = cmd.Title,
                    AreaOfUsage = cmd.AreaOfUsage,
                    ClinicalNeed = cmd.ClinicalNeed,
                    Gmdn = cmd.Gmdn,
                    PotentialTechnology = cmd.PotentialTechnology
                };

                DocumentSession.Events.Append(cmd.NewProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}