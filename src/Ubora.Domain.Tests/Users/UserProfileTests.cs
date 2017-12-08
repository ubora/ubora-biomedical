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

        [Theory]
        [InlineData("mentor", true)]
        [InlineData("not_mentor", false)]
        public void HasChosenMentorRole_Returns_Whether_Role_Is_Set_As_Mentor(
            string role, bool expected)
        {
            var userProfile = new UserProfile(userId: Guid.NewGuid())
                .Set(profile => profile.Role, role);

            // Act
            var result = userProfile.HasChosenMentorRole();

            // Assert
            result.Should().Be(expected);
        }
    }
}
