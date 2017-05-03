using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users
{
    public class EditUserProfileCommandHandler : ICommandHandler<EditUserProfileCommand>
    {
        private readonly IDocumentSession _documentSession;

        public EditUserProfileCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(EditUserProfileCommand command)
        {
            var userProfile = _documentSession.Load<UserProfile>(command.UserId);

            userProfile.FirstName = command.FirstName;
            userProfile.LastName = command.LastName;
            userProfile.University = command.University;
            userProfile.Degree = command.Degree;
            userProfile.Field = command.Field;
            userProfile.Biography = command.Biography;
            userProfile.Skills = command.Skills;
            userProfile.Role = command.Role;

            _documentSession.Store(userProfile);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}