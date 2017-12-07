using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users.Commands
{
    public class EditUserProfileCommand : UserCommand
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string CountryCode { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string University { get; set; }
        public string MedicalDevice { get; set; }
        public string Institution { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }

        internal class Handler : ICommandHandler<EditUserProfileCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(EditUserProfileCommand cmd)
            {
                var userProfile = _documentSession.LoadOrThrow<UserProfile>(cmd.UserId);

                userProfile.FirstName = cmd.FirstName;
                userProfile.LastName = cmd.LastName;
                userProfile.Biography = cmd.Biography;
                userProfile.Country = GetCountryValueObject(cmd.CountryCode);
                userProfile.Degree = cmd.Degree;
                userProfile.Field = cmd.Field;
                userProfile.University = cmd.University;
                userProfile.MedicalDevice = cmd.MedicalDevice;
                userProfile.Institution = cmd.Institution;
                userProfile.Skills = cmd.Skills;
                userProfile.Role = cmd.Role;

                _documentSession.Store(userProfile);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }

            private static Country GetCountryValueObject(string countryCode)
            {
                if (string.IsNullOrWhiteSpace(countryCode))
                {
                    return Country.CreateEmpty();
                }
                else
                {
                    return new Country(countryCode);
                }
            }
        }
    }
}