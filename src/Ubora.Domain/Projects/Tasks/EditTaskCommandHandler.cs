using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Tasks
{
    internal class EditTaskCommandHandler : CommandHandler<EditTaskCommand>
    {
        public EditTaskCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(EditTaskCommand cmd)
        {
            var project = DocumentSession.Load<Project>(cmd.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var @event = new TaskEditedEvent(cmd.UserInfo)
            {
                Description = cmd.Description,
                ProjectId = cmd.ProjectId,
                Id = cmd.Id,
                Title = cmd.Title
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}