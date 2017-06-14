using System;
using FluentAssertions;
using Ubora.Domain.Tests.Helper;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class UserProfileTests
    {
        [Fact]
        public void Constructor_Sets_UserId()
        {
            var userId = Guid.NewGuid();

            //Act
            var userProfile = new UserProfile(userId);

            //Assert
            userProfile.UserId.Should().Be(userId);
        }

        [Fact]
        public void SetProfilePictureBlobName_Sets_ProfilePictureBlobName()
        {
            var testfile = "testFile.png";
            var userId = Guid.NewGuid();
            var userProfile = new UserProfile(userId);
            var profilePictureBlobName = Guid.NewGuid() + testfile;
            
            //Act
            userProfile.SetProfilePictureBlobName(testfile);
            userProfile.SetPropertyValue(nameof(UserProfile.ProfilePictureBlobName), profilePictureBlobName);

            //Assert
            userProfile.UserId.Should().Be(userId);
            userProfile.ProfilePictureBlobName.Should().Be(profilePictureBlobName);
        }
    }
}
