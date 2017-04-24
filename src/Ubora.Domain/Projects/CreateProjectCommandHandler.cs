using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public CreateProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(CreateProjectCommand command)
        {
            _documentSession.Events.Append(command.Id, new ProjectCreatedEvent(command.Name, command.UserInfo));
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}