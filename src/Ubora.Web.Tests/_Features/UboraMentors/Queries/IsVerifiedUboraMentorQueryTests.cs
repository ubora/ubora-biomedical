using System;
using FluentAssertions;
using Moq;
using Ubora.Domain.Users.Queries;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.UboraMentors.Queries;
using Xunit;

namespace Ubora.Web.Tests._Features.UboraMentors.Queries
{
    public class IsVerifiedUboraMentorQueryTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Handle_Returns_Answer_For_Whether_User_Is_Verified_Ubora_Mentor_From_UserManager(
            bool isMentor)
        {
            var userId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId };

            var userManagerMock = new Mock<FakeUserManager>();

            userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(x => x.IsInRoleAsync(user, ApplicationRole.Mentor))
                .ReturnsAsync(isMentor);

            var query = new IsVerifiedUboraMentorQuery(userId);
            var handlerUnderTest = new IsVerifiedUboraMentorQueryHandler(userManagerMock.Object);

            // Act
            var result = handlerUnderTest.Handle(query);

            // Assert
            result.Should().Be(isMentor);
        }
    }
}
