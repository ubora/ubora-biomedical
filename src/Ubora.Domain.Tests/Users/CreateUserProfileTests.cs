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
            var command = new CreateUserProfileCommand
            {
                UserId = expectedUserId,
                FirstName = "expectedFirstName",
                LastName = "expectedLastName",
                Biography = "expectedBiography",
                Degree = "expectedDegree",
                Field = "expectedField",
                Skills = "expectedSkills",
                University = "expectedUniversity",
                Role = "expectedRole",
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
            createdUserProfile.Degree.Should().Be("expectedDegree");
            createdUserProfile.Field.Should().Be("expectedField");
            createdUserProfile.Skills.Should().Be("expectedSkills");
            createdUserProfile.University.Should().Be("expectedUniversity");
            createdUserProfile.Role.Should().Be("expectedRole");
        }
    }
}
