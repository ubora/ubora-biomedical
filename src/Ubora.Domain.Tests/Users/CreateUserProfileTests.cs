using System;
using Autofac;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class CreateUserProfileTests : IntegrationFixture
    {
        [Fact]
        public void UserProfile_Is_Created_With_Expected_Properties()
        {
            var expectedUserId = Guid.NewGuid();
            var expectedDateOfBirth = DateTime.Now.AddDays(-1);
            var expectedGender = (Gender)123;
            var command = new CreateUserProfileCommand
            {
                UserId = expectedUserId,
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
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            // Act
            var result = commandProcessor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var createdUserProfile = Session.Load<UserProfile>(expectedUserId);

            createdUserProfile.FirstName.Should().Be("expectedFirstName");
            createdUserProfile.LastName.Should().Be("expectedLastName");
            createdUserProfile.Biography.Should().Be("expectedBiography");
            createdUserProfile.Country.Should().Be("expectedCountry");
            createdUserProfile.DateOfBirth.Should().Be(expectedDateOfBirth);
            createdUserProfile.Degree.Should().Be("expectedDegree");
            createdUserProfile.Field.Should().Be("expectedField");
            createdUserProfile.Gender.Should().Be(expectedGender);
            createdUserProfile.Skills.Should().Be("expectedSkills");
            createdUserProfile.University.Should().Be("expectedUniversity");
        }
    }
}
