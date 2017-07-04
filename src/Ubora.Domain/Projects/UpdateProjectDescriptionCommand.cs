using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class UpdateProjectDescriptionCommand : UserProjectCommand
    {
        public string Description { get; set; }
    }

    internal class UpdateProjectDescriptionCommandHandler : CommandHandler<UpdateProjectDescriptionCommand>
    {
        public UpdateProjectDescriptionCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(UpdateProjectDescriptionCommand cmd)
        {
            var project = DocumentSession.Load<Project>(cmd.ProjectId);

            var @event = new EditProjectDescriptionEvent(cmd.Actor)
            {
                Id = cmd.ProjectId,
                Description = cmd.Description
            };

            DocumentSession.Events.Append(project.Id, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
