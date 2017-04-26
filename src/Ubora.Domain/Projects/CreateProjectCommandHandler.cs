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
            var @event = new ProjectCreatedEvent(command.UserInfo, command.Title, command.Description, command.ClinicalNeed, command.AreaOfUsage, command.PotentialTechnology, command.GmdnTerm, command.GmdnDefinition, command.GmdnCode);
            _documentSession.Events.Append(command.Id, @event);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}