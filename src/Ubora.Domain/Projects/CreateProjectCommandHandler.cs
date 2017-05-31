using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects
{
    internal class CreateProjectCommandHandler : CommandHandler<CreateProjectCommand>
    {
        public CreateProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(CreateProjectCommand cmd)
        {
            var projectEvent = new ProjectCreatedEvent(cmd.Actor)
            {
                Id = cmd.NewProjectId,
                Title = cmd.Title,
                AreaOfUsage = cmd.AreaOfUsage,
                ClinicalNeed = cmd.ClinicalNeed,
                Gmdn = cmd.Gmdn,
                PotentialTechnology = cmd.PotentialTechnology
            };

            var workpackageEvent = new WorkpackageOneOpenedEvent(cmd.Actor)
            {
                ProjectId = cmd.NewProjectId,
            };

            DocumentSession.Events.Append(cmd.NewProjectId, projectEvent);
            DocumentSession.Events.Append(cmd.NewProjectId, workpackageEvent);

            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}