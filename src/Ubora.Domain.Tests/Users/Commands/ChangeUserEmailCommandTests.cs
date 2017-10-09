using Autofac;
using FluentAssertions;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Users.Commands
{
    public class ChangeUserEmailCommandTests : IntegrationFixture
    {
        [Fact]
        public void Command_Sets_New_Email_To_User()
        {
            var userId = Guid.NewGuid();
            CreateExistingUserProfile(userId);

            var newEmail = "newEmail@agileworks.eu";
            var command = new ChangeUserEmailCommand
            {
                UserId = userId,
                Email = newEmail
            };
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            // Act
            var result = commandProcessor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var updatedUserProfile = Session.Load<UserProfile>(userId);

            updatedUserProfile.Email.Should().Be(newEmail);

        }

        private void CreateExistingUserProfile(Guid userId)
        {
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            commandProcessor.Execute(new CreateUserProfileCommand
            {
                UserId = userId,
                Email = "oldEmail@agileworks.eu"
            });
        }
    }
}
