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
using Ubora.Web.Tests.Helper;
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
        private readonly Mock<ICommandQueryProcessor> _commandProcessorMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<IAuthMessageSender> _authMessageSenderMock;
        private readonly AccountController _controller;
        public const string UserId = "testUserId";
        public const string Code = "testCode";

        public AccountControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _identityCookieOptionsMock = new Mock<IOptions<IdentityCookieOptions>>();
            _identityCookieOptionsMock.Setup(o => o.Value).Returns(new IdentityCookieOptions());
            _emailSenderMock = new Mock<IEmailSender>();
            _commandProcessorMock = new Mock<ICommandQueryProcessor>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _authMessageSenderMock = new Mock<IAuthMessageSender>(MockBehavior.Strict);
            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object,
                _identityCookieOptionsMock.Object, _emailSenderMock.Object,
                _loggerFactoryMock.Object, _commandProcessorMock.Object, _authMessageSenderMock.Object);
        }

        [Fact]
        public async void ConfirmEmail_Returns_Error_View_When_UserId_Is_Null_Or_Code_Is_Null()
        {
            //Act
            var result = await _controller.ConfirmEmail(null, null) as ViewResult;

            //Assert
            Assert.Equal(result.ViewName, "Error");
            _userManagerMock.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async void ConfirmEmail_Returns_Error_View_If_Not_Found_User_In_Manager()
        {
            _userManagerMock.Setup(x => x.FindByIdAsync(UserId)).Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            var result = await _controller.ConfirmEmail(UserId, Code) as ViewResult;

            //Assert
            Assert.Equal(result.ViewName, "Error");
            _userManagerMock.Verify(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async void ConfirmEmail_Confirms_And_Returns_CorfirmEmail_View()
        {
            var identityResult = new IdentityResult();
            identityResult.Set(nameof(IdentityResult.Succeeded), true);

            _userManagerMock.Setup(x => x.FindByIdAsync(UserId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), Code))
                .ReturnsAsync(identityResult);

            //Act
            var result = await _controller.ConfirmEmail(UserId, Code) as ViewResult;

            //Assert
            Assert.Equal(result.ViewName, "ConfirmEmail");
        }

        [Fact]
        public async void ConfirmEmail_Returns_Error_View_When_Not_Confirms()
        {
            var identityResult = new IdentityResult();
            identityResult.Set(nameof(IdentityResult.Succeeded), false);

            _userManagerMock.Setup(x => x.FindByIdAsync(UserId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), Code))
                .ReturnsAsync(identityResult);

            //Act
            var result = await _controller.ConfirmEmail(UserId, Code) as ViewResult;

            //Assert
            Assert.Equal(result.ViewName, "Error");
        }

        [Fact]
        public async void ForgotPassword_Returns_View_And_Model_When_ModelState_Is_Invalid()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel()
            {
                Email = "test"
            };
            _controller.ViewData.ModelState.AddModelError("", "mock error message");

            //Act
            var result = await _controller.ForgotPassword(forgotPasswordViewModel) as ViewResult;

            //Assert
            result.Model.Should().Be(forgotPasswordViewModel);
        }

        [Fact]
        public async void ForgotPassword_Returns_ForgotPasswordConfirmation_View_If_User_Is_Not_Found()
        {
            var forgotpasswordviewmodel = new ForgotPasswordViewModel()
            {
                Email = "test@test.com"
            };

            var identity = new ApplicationUser();

            _userManagerMock.Setup(x => x.FindByNameAsync(forgotpasswordviewmodel.Email))
                .Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            var result = await _controller.ForgotPassword(forgotpasswordviewmodel) as ViewResult;

            //Assert
            result.ViewName.Should().Be("ForgotPasswordConfirmation");
            _userManagerMock.Verify(x => x.IsEmailConfirmedAsync(identity), Times.Never);
        }

        [Fact]
        public async void ForgotPassword_Sends_Password_Reset_Mail_To_User_And_Results_ForgotPasswordConfirmation_View()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel()
            {
                Email = "test@test.com"
            };
            var identity = new ApplicationUser();

            _userManagerMock.Setup(x => x.FindByNameAsync(forgotPasswordViewModel.Email))
                .ReturnsAsync(identity);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(identity)).ReturnsAsync(true);
            _authMessageSenderMock.Setup(x => x.SendForgotPasswordMessageAsync(identity)).Returns(Task.FromResult(identity));
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Scheme = "http";


            //Act
            var result = await _controller.ForgotPassword(forgotPasswordViewModel) as ViewResult;

            //Assert
            result.ViewName.Should().Be("ForgotPasswordConfirmation");
        }
    }
}
