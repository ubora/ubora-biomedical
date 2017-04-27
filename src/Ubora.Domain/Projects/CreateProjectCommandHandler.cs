using AutoMapper;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IDocumentSession documentSession, IMapper mapper)
        {
            _documentSession = documentSession;
            _mapper = mapper;
        }

        public ICommandResult Handle(CreateProjectCommand cmd)
        {
            var evnt = new ProjectCreatedEvent(cmd.UserInfo);
            _mapper.Map(cmd, evnt);

            _documentSession.Events.Append(cmd.ProjectId, evnt);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}