using System;
using FluentAssertions;
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
    }
}
