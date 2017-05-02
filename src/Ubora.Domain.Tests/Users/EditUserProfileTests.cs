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
                Degree = "expectedDegree",
                Field = "expectedField",
                Skills = "expectedSkills",
                University = "expectedUniversity",
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
            updatedUserProfile.Degree.Should().Be("expectedDegree");
            updatedUserProfile.Field.Should().Be("expectedField");
            updatedUserProfile.Skills.Should().Be("expectedSkills");
            updatedUserProfile.University.Should().Be("expectedUniversity");
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
