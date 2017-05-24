using System;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Services;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Users.Account;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.Account
{
    public class AccountControllerTests
    {
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<IOptions<IdentityCookieOptions>> _identityCookieOptionsMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<ISmsSender> _smsSenderMock;
        private readonly Mock<ICommandProcessor> _commandProcessorMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _identityCookieOptionsMock = new Mock<IOptions<IdentityCookieOptions>>();
            _emailSenderMock = new Mock<IEmailSender>();
            _smsSenderMock = new Mock<ISmsSender>();
            _commandProcessorMock = new Mock<ICommandProcessor>();
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object,
                _identityCookieOptionsMock.Object, _emailSenderMock.Object, _smsSenderMock.Object,
                _loggerFactoryMock.Object, _commandProcessorMock.Object, _queryProcessorMock.Object);

        }

        [Fact]
        public void ViewProfile_Returns_View_And_ProfileViewModel_When_Should_Have_User()
        {
            var expecteduserId = Guid.NewGuid();

            _queryProcessorMock.Setup(p => p.FindById<UserProfile>(expecteduserId))
                .Returns(new UserProfile(expecteduserId));

            //Act
            var result = _controller.ViewProfile(expecteduserId);

            //Assert
            result.Should().NotBeNull();
        }
    }
}
