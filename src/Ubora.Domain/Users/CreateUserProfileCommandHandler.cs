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

        public ICommandResult Handle(CreateUserProfileCommand cmd)
        {
            var userProfile = new UserProfile(cmd.UserId)
            {
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
                University = cmd.University,
                Degree = cmd.Degree,
                Field = cmd.Field,
                Biography = cmd.Biography,
                Skills = cmd.Skills,
                Role = cmd.Role
            };

            _documentSession.Store(userProfile);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}