using System;
using System.IO;
using Autofac;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class ChangeUserProfilePictureTests : IntegrationFixture
    {
        [Fact]
        public void UserProfile_Is_Changed_With_BlobName()
        {
            var userId = Guid.NewGuid();
            CreateExistingUserProfile(userId);

            var command = new ChangeUserProfilePictureCommand
            {
                UserId = userId,
                FileStream = Stream.Null
            };
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            // Act
            var result = commandProcessor.Execute(command);

            var changedUserProfile = Session.Load<UserProfile>(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
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
