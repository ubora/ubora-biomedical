using System;
using System.IO;
using System.Linq;
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
        public void UserProfile_Is_Changed_With_ProfilePictureBlobName()
        {
            Guid parsedGuid;
            var userId = Guid.NewGuid();
            CreateExistingUserProfile(userId);

            var command = new ChangeUserProfilePictureCommand
            {
                UserId = userId,
                Stream = Stream.Null,
                FileName = "testImage.jpg"
            };
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            // Act
            var result = commandProcessor.Execute(command);

            var changedUserProfile = Session.Load<UserProfile>(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            changedUserProfile.ProfilePictureBlobName.Contains(command.FileName).Should().BeTrue();
            var guidPart = changedUserProfile.ProfilePictureBlobName
                .Split(new[] { command.FileName }, StringSplitOptions.RemoveEmptyEntries)
                .First();
            Guid.TryParse(guidPart, out parsedGuid).Should().BeTrue();
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
