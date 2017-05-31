using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    internal class UpdateProjectCommandHandler : CommandHandler<UpdateProjectCommand>
    {
        public UpdateProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(UpdateProjectCommand cmd)
        {
            var project = DocumentSession.Load<Project>(cmd.ProjectId);

            var @event = new ProjectUpdatedEvent(cmd.Actor)
            {
                Id = cmd.ProjectId,
                Title = cmd.Title,
                ClinicalNeedTags = cmd.ClinicalNeedTags,
                AreaOfUsageTags = cmd.AreaOfUsageTags,
                PotentialTechnologyTags = cmd.PotentialTechnologyTags,
                Gmdn = cmd.Gmdn
            };

            DocumentSession.Events.Append(project.Id, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}