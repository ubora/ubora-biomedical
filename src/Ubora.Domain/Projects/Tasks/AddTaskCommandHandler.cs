using AutoMapper;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Tasks
{
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
}