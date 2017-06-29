using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    internal class CreateProjectCommandHandler : CommandHandler<CreateProjectCommand>
    {
        public CreateProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(CreateProjectCommand cmd)
        {
            var project = DocumentSession.Load<Project>(cmd.NewProjectId);
            if (project != null)
            {
                throw new InvalidOperationException();
            }

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

            return new CommandResult();
        }
    }
}