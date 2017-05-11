using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Tasks
{
    internal class AddTaskCommandHandler : CommandHandler<AddTaskCommand>
    {
        public AddTaskCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(AddTaskCommand cmd)
        {
            var project = DocumentSession.Load<Project>(cmd.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var @event = new TaskAddedEvent(cmd.UserInfo)
            {
                ProjectId = cmd.ProjectId,
                Id = cmd.Id,
                Title = cmd.Title,
                Description = cmd.Description
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}