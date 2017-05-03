using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Tasks
{
    public class EditTaskCommand : ICommand
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserInfo InitiatedBy { get; set; }
    }

    public class TaskEditedEvent : UboraEvent, ITaskEvent
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public override string GetDescription()
        {
            return "test";
        }

        public TaskEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }
    }

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

            var e = new TaskEditedEvent(command.InitiatedBy)
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
