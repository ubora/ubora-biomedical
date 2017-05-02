using System;
using AutoMapper;
using Marten;
using Marten.Schema;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Tasks
{
    public class AddTaskCommand : ICommand
    {
        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserInfo InitiatedBy { get; set; }
    }

    public class AddTaskCommandHandler : ICommandHandler<AddTaskCommand>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IMapper _mapper;

        public AddTaskCommandHandler(IDocumentSession documentSession, IMapper mapper)
        {
            _documentSession = documentSession;
            _mapper = mapper;
        }

        public ICommandResult Handle(AddTaskCommand command)
        {
            var e = new TaskAddedEvent(command.InitiatedBy);
            _mapper.Map(command, e);

            _documentSession.Events.Append(command.ProjectId, e);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }

    public class TaskAddedEvent : UboraEvent
    {
        public TaskAddedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }

    public class ProjectTask
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        private void Apply(TaskAddedEvent e)
        {
            Id = e.TaskId;
            ProjectId = e.ProjectId;
            Title = e.Title;
            Description = e.Description;
        }
    }
}
