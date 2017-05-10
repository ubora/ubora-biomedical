using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Tasks
{
    public class EditTaskCommandHandler : ICommandHandler<EditTaskCommand>
    {
        private readonly IDocumentSession _documentSession;

        public EditTaskCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(EditTaskCommand command)
        {
            var project = _documentSession.Load<Project>(command.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var e = new TaskEditedEvent(command.UserInfo)
            {
                Description = command.Description,
                ProjectId = command.ProjectId,
                Id = command.Id,
                Title = command.Title
            };

            _documentSession.Events.Append(command.ProjectId, e);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}