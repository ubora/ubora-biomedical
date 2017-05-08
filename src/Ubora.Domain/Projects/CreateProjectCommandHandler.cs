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
            var @event = new ProjectCreatedEvent(command.UserInfo)
            {
                Id = command.Id,
                Title = command.Title,
                AreaOfUsage = command.AreaOfUsage,
                ClinicalNeed = command.ClinicalNeed,
                GmdnCode = command.GmdnCode,
                GmdnDefinition = command.GmdnDefinition,
                GmdnTerm = command.GmdnTerm,
                PotentialTechnology = command.PotentialTechnology
            };

            _documentSession.Events.Append(command.Id, @event);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}