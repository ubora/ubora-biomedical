using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class UpdateProjectDescriptionCommand : UserProjectCommand
    {
        public string Description { get; set; }

        internal class Handler : CommandHandler<UpdateProjectDescriptionCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
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
}
