using System;
using Autofac;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Xunit;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Users.Commands;

namespace Ubora.Domain.Tests.Users
{
    public class ChangeUserProfilePictureTests : IntegrationFixture
    {
        [Fact]
        public void UserProfile_Is_Changed_With_ProfilePictureBlobName()
        {
            var userId = Guid.NewGuid();
            CreateExistingUserProfile(userId);
            var expectedBlobLocation = new BlobLocation("test", "test.jpg");
            var command = new ChangeUserProfilePictureCommand
            {
                BlobLocation = expectedBlobLocation,
                Actor = new UserInfo(userId, "testuser")
            };

            // Act
            var result = this.Processor.Execute(command);

            var changedUserProfile = Session.Load<UserProfile>(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            changedUserProfile.ProfilePictureBlobLocation.Should().Be(expectedBlobLocation);
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
