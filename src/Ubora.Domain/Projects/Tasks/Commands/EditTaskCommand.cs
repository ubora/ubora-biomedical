using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Tasks.Events;

namespace Ubora.Domain.Projects.Tasks.Commands
{
    public class EditTaskCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        internal class Handler : CommandHandler<EditTaskCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditTaskCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var @event = new TaskEditedEvent(
                    initiatedBy: cmd.Actor,
                    description: cmd.Description,
                    projectId: cmd.ProjectId,
                    id: cmd.Id,
                    title: cmd.Title);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
