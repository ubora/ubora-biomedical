using System;
using Autofac;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class EditUserProfileTests : IntegrationFixture
    {
        [Fact]
        public void UserProfile_Is_Updated_With_Expected_Properties()
        {
            var userId = Guid.NewGuid();
            CreateExistingUserProfile(userId);

            var command = new EditUserProfileCommand
            {
                UserId = userId,
                FirstName = "expectedFirstName",
                LastName = "expectedLastName",
                Biography = "expectedBiography",
                CountryCode = "expectedCountry",
                Degree = "expectedDegree",
                Field = "expectedField",
                University = "expectedUniversity",
                MedicalDevice = "expectedMedicalDevice",
                Institution = "expectedInstitution",
                Skills = "expectedSkills",
                Role = "expectedRole"
            };
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            // Act
            var result = commandProcessor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var updatedUserProfile = Session.Load<UserProfile>(userId);

            updatedUserProfile.FirstName.Should().Be("expectedFirstName");
            updatedUserProfile.LastName.Should().Be("expectedLastName");
            updatedUserProfile.Biography.Should().Be("expectedBiography");
            updatedUserProfile.Country.Code.Should().Be("expectedCountry");
            updatedUserProfile.Degree.Should().Be("expectedDegree");
            updatedUserProfile.Field.Should().Be("expectedField");
            updatedUserProfile.University.Should().Be("expectedUniversity");
            updatedUserProfile.MedicalDevice.Should().Be("expectedMedicalDevice");
            updatedUserProfile.Institution.Should().Be("expectedInstitution");
            updatedUserProfile.Skills.Should().Be("expectedSkills");
            updatedUserProfile.Role.Should().Be("expectedRole");
        }

        private void CreateExistingUserProfile(Guid userId)
        {
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            commandProcessor.Execute(new CreateUserProfileCommand
            {
                UserId = userId
            });
        }
    }
}
