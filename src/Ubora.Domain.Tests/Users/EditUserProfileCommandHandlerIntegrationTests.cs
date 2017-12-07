using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class EditUserProfileCommandHandlerIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void UserProfile_Can_Be_Edited()
        {
            var userId = Guid.NewGuid();

            var editProfileCommand = new EditUserProfileCommand
            {
                UserId = userId,
                FirstName = "editedFirstName",
                LastName = "editedLastName",
                Biography = "editedBiography",
                CountryCode = "EST",
                Degree = "editedDegree",
                Field = "editedField",
                University = "editedUniversity",
                MedicalDevice = "editedMedicalDevice",
                Institution = "editedInstitution",
                Skills = "editedSkills",
                Role = "editedRole",
                Actor = new DummyUserInfo()
            };

            this.Given(_ => this.Create_User(userId, "email", "firstName", "lastName"))
                .When(_ => Processor.Execute(editProfileCommand))
                .Then(_ => Assert_User_Profile_Is_Edited(editProfileCommand))
                .BDDfy();
        }

        private void Assert_User_Profile_Is_Edited(EditUserProfileCommand command)
        {
            var userProfile = Processor.FindById<UserProfile>(command.UserId);
            userProfile.Should().NotBeNull();

            userProfile.FirstName.Should().Be(command.FirstName);
            userProfile.LastName.Should().Be(command.LastName);
            userProfile.Biography.Should().Be(command.Biography);
            userProfile.Country.Code.Should().Be(command.CountryCode);
            userProfile.Degree.Should().Be(command.Degree);
            userProfile.Field.Should().Be(command.Field);
            userProfile.University.Should().Be(command.University);
            userProfile.MedicalDevice.Should().Be(command.MedicalDevice);
            userProfile.Institution.Should().Be(command.Institution);
            userProfile.Skills.Should().Be(command.Skills);
            userProfile.Role.Should().Be(command.Role);
        }
    }
}