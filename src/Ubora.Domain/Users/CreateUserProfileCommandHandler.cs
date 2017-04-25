using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;

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
            var userProfile = new UserProfile
            {
                UserId = command.UserId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                University = command.University,
                Degree = command.Degree,
                Field = command.Field,
                DateOfBirth = command.DateOfBirth,
                Gender = command.Gender,
                Country = command.Country,
                Biography = command.Biography,
                Skills = command.Skills
            };
            _documentSession.Store(userProfile);
            _documentSession.SaveChanges();
            return new CommandResult(true);
        }
    }
}