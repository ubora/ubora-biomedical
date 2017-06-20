using System;
using FluentAssertions;
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
            var command = new CreateUserProfileCommand
            {
                UserId = expectedUserId,
                Email = "expectedEmail",
                FirstName = "expectedFirstName",
                LastName = "expectedLastName",
                Biography = "expectedBiography",
                Degree = "expectedDegree",
                Field = "expectedField",
                Skills = "expectedSkills",
                University = "expectedUniversity",
                Role = "expectedRole"
            };

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var createdUserProfile = Session.Load<UserProfile>(expectedUserId);

            createdUserProfile.Email.Should().Be("expectedEmail");
            createdUserProfile.FirstName.Should().Be("expectedFirstName");
            createdUserProfile.LastName.Should().Be("expectedLastName");
            createdUserProfile.Biography.Should().Be("expectedBiography");
            createdUserProfile.Degree.Should().Be("expectedDegree");
            createdUserProfile.Field.Should().Be("expectedField");
            createdUserProfile.Skills.Should().Be("expectedSkills");
            createdUserProfile.University.Should().Be("expectedUniversity");
            createdUserProfile.Role.Should().Be("expectedRole");
            createdUserProfile.ProfilePictureBlobName.Should().Be("Default");
        }
    }
}
