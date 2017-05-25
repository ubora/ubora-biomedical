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
            var @event = new ProjectCreatedEvent(cmd.Actor)
            {
                Id = cmd.NewProjectId,
                Title = cmd.Title,
                AreaOfUsage = cmd.AreaOfUsage,
                ClinicalNeed = cmd.ClinicalNeed,
                GmdnCode = cmd.GmdnCode,
                GmdnDefinition = cmd.GmdnDefinition,
                GmdnTerm = cmd.GmdnTerm,
                PotentialTechnology = cmd.PotentialTechnology
            };

            DocumentSession.Events.Append(cmd.NewProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}