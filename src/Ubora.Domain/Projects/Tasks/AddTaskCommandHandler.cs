using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Tasks
{
    public class AddTaskCommandHandler : ICommandHandler<AddTaskCommand>
    {
        private readonly IDocumentSession _documentSession;

        public AddTaskCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(AddTaskCommand command)
        {
            var project = _documentSession.Load<Project>(command.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var @event = new TaskAddedEvent(command.InitiatedBy)
            {
                ProjectId = command.ProjectId,
                Id = command.Id,
                Title = command.Title,
                Description = command.Description
            };

            _documentSession.Events.Append(command.ProjectId, @event);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}