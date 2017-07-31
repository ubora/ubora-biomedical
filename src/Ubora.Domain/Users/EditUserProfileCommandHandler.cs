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

        public ICommandResult Handle(EditUserProfileCommand cmd)
        {
            var userProfile = _documentSession.Load<UserProfile>(cmd.UserId);

            userProfile.FirstName = cmd.FirstName;
            userProfile.LastName = cmd.LastName;
            userProfile.Biography = cmd.Biography;
            userProfile.Country = new Country(cmd.CountryCode);
            userProfile.Degree = cmd.Degree;
            userProfile.Field = cmd.Field;
            userProfile.University = cmd.University;
            userProfile.MedicalDevice = cmd.MedicalDevice;
            userProfile.Institution = cmd.Institution;
            userProfile.Skills = cmd.Skills;
            userProfile.Role = cmd.Role;
            userProfile.IsFirstTimeEditedProfile = cmd.IsFirstTimeEditedProfile;

            _documentSession.Store(userProfile);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}