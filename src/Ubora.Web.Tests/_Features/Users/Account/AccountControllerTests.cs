using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Web.Data;
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
        private readonly Mock<ICommandQueryProcessor> _commandProcessorMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<IAuthMessageSender> _authMessageSenderMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _identityCookieOptionsMock = new Mock<IOptions<IdentityCookieOptions>>();
            _identityCookieOptionsMock.Setup(o => o.Value).Returns(new IdentityCookieOptions());
            _emailSenderMock = new Mock<IEmailSender>();
            _smsSenderMock = new Mock<ISmsSender>();
            _commandProcessorMock = new Mock<ICommandQueryProcessor>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _authMessageSenderMock = new Mock<IAuthMessageSender>();
            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object,
                _identityCookieOptionsMock.Object, _emailSenderMock.Object, _smsSenderMock.Object,
                _loggerFactoryMock.Object, _commandProcessorMock.Object, _authMessageSenderMock.Object);
        }

        [Fact]
        public async void ForgetPassword_Returns_View_And_Model_When_ModelState_Is_Invalid()
        {
            var forgotpasswordviewmodel = new ForgotPasswordViewModel()
            {
                Email = "test"
            };

            //Act
            _controller.ViewData.ModelState.AddModelError("", "mock error message");
            var result = await _controller.ForgotPassword(forgotpasswordviewmodel) as ViewResult;

            //Assert
            result.Model.Should().Be(forgotpasswordviewmodel);
            _authMessageSenderMock.Verify(x => x.SendForgotPasswordMessage(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async void ForgetPassword_Returns_ForgotPasswordConfirmation_View_If_Not_Found_User_In_Manager()
        {
            var forgotpasswordviewmodel = new ForgotPasswordViewModel()
            {
                Email = "test@test.com"
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(forgotpasswordviewmodel.Email))
                .Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            var result = await _controller.ForgotPassword(forgotpasswordviewmodel) as ViewResult;

            //Assert
            result.ViewName.Should().Be("ForgotPasswordConfirmation");
            _userManagerMock.Verify(x => x.IsEmailConfirmedAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _authMessageSenderMock.Verify(x => x.SendForgotPasswordMessage(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async void ForgetPassword_Sends_Password_Reset_Mail_To_User_And_Results_ForgotPasswordConfirmation_View()
        {
            var forgotpasswordviewmodel = new ForgotPasswordViewModel()
            {
                Email = "test@test.com"
            };
            var identity = new ApplicationUser();

            _userManagerMock.Setup(x => x.FindByNameAsync(forgotpasswordviewmodel.Email))
                .ReturnsAsync(identity);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(true);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Scheme = "http";

            //Act
            var result = await _controller.ForgotPassword(forgotpasswordviewmodel) as ViewResult;

            //Assert
            result.ViewName.Should().Be("ForgotPasswordConfirmation");
            _authMessageSenderMock.Verify(
                x =>
                    x.SendForgotPasswordMessage(identity), Times.Once);
        }
    }
}
