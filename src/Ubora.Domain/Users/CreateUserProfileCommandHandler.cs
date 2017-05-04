using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users
{
    public class CreateUserProfileCommandHandler : ICommandHandler<CreateUserProfileCommand>
    {
        private readonly IDocumentSession _documentSession;

        public CreateUserProfileCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(CreateUserProfileCommand command)
        {
            var userProfile = new UserProfile(command.UserId)
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                University = command.University,
                Degree = command.Degree,
                Field = command.Field,
                Biography = command.Biography,
                Skills = command.Skills,
                Role = command.Role
            };

            _documentSession.Store(userProfile);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}