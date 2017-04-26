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
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            var userId = Guid.NewGuid();
            commandProcessor.Execute(new CreateUserProfileCommand
            {
                UserId = userId
            });

            var expectedDateOfBirth = DateTime.Now.AddDays(-1);
            var expectedGender = (Gender)123;
            var command = new EditUserProfileCommand
            {
                UserId = userId,
                FirstName = "expectedFirstName",
                LastName = "expectedLastName",
                Biography = "expectedBiography",
                Country = "expectedCountry",
                DateOfBirth = expectedDateOfBirth,
                Degree = "expectedDegree",
                Field = "expectedField",
                Gender = expectedGender,
                Skills = "expectedSkills",
                University = "expectedUniversity",
            };

            // Act
            var result = commandProcessor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var updatedUserProfile = Session.Load<UserProfile>(userId);

            updatedUserProfile.FirstName.Should().Be("expectedFirstName");
            updatedUserProfile.LastName.Should().Be("expectedLastName");
            updatedUserProfile.Biography.Should().Be("expectedBiography");
            updatedUserProfile.Country.Should().Be("expectedCountry");
            updatedUserProfile.DateOfBirth.Should().Be(expectedDateOfBirth);
            updatedUserProfile.Degree.Should().Be("expectedDegree");
            updatedUserProfile.Field.Should().Be("expectedField");
            updatedUserProfile.Gender.Should().Be(expectedGender);
            updatedUserProfile.Skills.Should().Be("expectedSkills");
            updatedUserProfile.University.Should().Be("expectedUniversity");
        }
    }
}
