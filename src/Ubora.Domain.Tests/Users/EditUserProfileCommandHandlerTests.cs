using System;
using System.Linq;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class EditUserProfileCommandHandlerTests
    {
        private readonly EditUserProfileCommand.Handler _handlerUnderTest;
        private readonly Mock<IDocumentSession> _documentSessionMock;

        public EditUserProfileCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _handlerUnderTest = new EditUserProfileCommand.Handler(_documentSessionMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Handle_Sets_Country_As_Empty_When_Not_Specified_In_Command(
            string countryCode)
        {
            var userProfile = new UserProfile(userId: Guid.NewGuid());

            _documentSessionMock
                .Setup(x => x.Load<UserProfile>(userProfile.UserId))
                .Returns(userProfile);

            var command = new EditUserProfileCommand
            {
                UserId = userProfile.UserId,
                CountryCode = countryCode
            };

            UserProfile storedUserProfile = null;
            _documentSessionMock
                .Setup(x => x.Store(It.IsAny<UserProfile[]>()))
                .Callback<UserProfile[]>(x => storedUserProfile = x.Single());

            // Act
            _handlerUnderTest.Handle(command);

            // Assert
            var emptyCountryCode = Country.CreateEmpty().Code;
            storedUserProfile.Country.Code.Should().Be(emptyCountryCode);
        }
    }
}