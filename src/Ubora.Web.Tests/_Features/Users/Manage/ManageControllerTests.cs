using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Ubora.Web.Services;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Users.Manage;
using Xunit;
using Ubora.Web.Data;
using System.Threading.Tasks;
using Ubora.Web._Features._Shared.Notices;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Features.Users.Account;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users.Commands;

namespace Ubora.Web.Tests._Features.Users.Manage
{
    public class ManageControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<EmailSender> _emailSenderMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<ILogger<ManageController>> _logger;
        private readonly Mock<IEmailConfirmationMessageSender> _confirmationMessageSenderMock;
        private readonly ManageController _controller;

        public ManageControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _emailSenderMock = new Mock<EmailSender>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _logger = new Mock<ILogger<ManageController>>();
            _confirmationMessageSenderMock = new Mock<IEmailConfirmationMessageSender>();

            _loggerFactoryMock.Setup(x => x.CreateLogger("ManageController"))
                .Returns(_logger.Object);

            _controller = new ManageController(_userManagerMock.Object, _signInManagerMock.Object, _emailSenderMock.Object, _loggerFactoryMock.Object, _confirmationMessageSenderMock.Object);
            SetUpForTest(_controller);
        }

        [Fact]
        public async Task ChangePassword_Changes_User_Password_And_Shows_Notification_About_It_When_Validated_Successfully()
        {
            var currentPassword = "currentPassword";
            var newPassword = "newPassword";
            var viewModel = new ChangePasswordViewModel
            {
                OldPassword = currentPassword,
                NewPassword = newPassword,
                ConfirmPassword = newPassword
            };

            var applicationUser = new ApplicationUser
            {
                UserName = "TestUserName"
            };

            _userManagerMock.Setup(x => x.GetUserAsync(_controller.HttpContext.User))
                .ReturnsAsync(applicationUser);

            var identitySuccessResult = IdentityResult.Success;
            _userManagerMock.Setup(x => x.ChangePasswordAsync(applicationUser, currentPassword, newPassword))
                .ReturnsAsync(identitySuccessResult);

            // SetTempData(_controller);

            // Act
            var result = (RedirectToActionResult)await _controller.ChangePassword(viewModel);

            // Assert
            _userManagerMock
                .Verify(x => x.ChangePasswordAsync(applicationUser, currentPassword, newPassword), Times.Once);

            var successNotice = _controller.Notices.Dequeue();
            successNotice.Text.Should().Be(SuccessTexts.PasswordChanged);
            successNotice.Type.Should().Be(NoticeType.Success);

            _signInManagerMock
               .Verify(x => x.SignInAsync(applicationUser, false, null), Times.Once);

            AssertRedirectToIndex(result);
        }

        [Fact]
        public async Task ChangePassword_Shows_Notification_And_Redirects_To_Index_When_User_Is_Not_Found()
        {
            var currentPassword = "currentPassword";
            var newPassword = "newPassword";
            var viewModel = new ChangePasswordViewModel
            {
                OldPassword = currentPassword,
                NewPassword = newPassword,
                ConfirmPassword = newPassword
            };

            _userManagerMock.Setup(x => x.GetUserAsync(_controller.HttpContext.User))
                .ReturnsAsync((ApplicationUser)null);

            //SetTempData(_controller);

            // Act
            var result = (RedirectToActionResult)await _controller.ChangePassword(viewModel);

            // Assert
            _userManagerMock
                .Verify(x => x.ChangePasswordAsync(It.IsAny<ApplicationUser>(), currentPassword, newPassword), Times.Never);

            _signInManagerMock
               .Verify(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null), Times.Never);

            var successNotice = _controller.Notices.Dequeue();
            successNotice.Text.Should().Be("Password could not be changed!");
            successNotice.Type.Should().Be(NoticeType.Error);

            AssertRedirectToIndex(result);
        }

        [Fact]
        public async Task ChangeEmail_Shows_Success_Notice_When_Email_Changed_Successfully()
        {
            var password = "password";
            var newEmail = "newEmail@gmail.com";
            var model = new ChangeEmailViewModel
            {
                NewEmail = newEmail,
                Password = password
            };

            var applicationUser = new ApplicationUser
            {
                UserName = "email@gmail.com"
            };

            _userManagerMock.Setup(x => x.GetUserAsync(_controller.HttpContext.User))
                .ReturnsAsync(applicationUser);

            _userManagerMock.Setup(x => x.CheckPasswordAsync(applicationUser, password))
                .ReturnsAsync(true);

            applicationUser.Email = newEmail;
            applicationUser.UserName = newEmail;
            applicationUser.EmailConfirmed = false;

            var identitySuccessResult = IdentityResult.Success;
            _userManagerMock.Setup(x => x.UpdateAsync(applicationUser))
                .ReturnsAsync(identitySuccessResult);

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeUserEmailCommand>()))
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)await _controller.ChangeEmail(model);

            // Assert
            _signInManagerMock.Verify(x => x.RefreshSignInAsync(applicationUser), Times.Once);
            _confirmationMessageSenderMock.Verify(x => x.SendEmailConfirmationMessage(applicationUser), Times.Once);

            var successNotice = _controller.Notices.Dequeue();
            successNotice.Text.Should().Be("Email was changed successfully!");
            successNotice.Type.Should().Be(NoticeType.Success);

            AssertRedirectToIndex(result);
        }

        [Fact]
        public async Task ChangeEmail_Returns_ChangeEmail_View_When_ModelState_Is_Invalid()
        {
            var model = new ChangeEmailViewModel();
            _controller.ModelState.AddModelError("key", "message");

            // Act
            var result = (ViewResult)await _controller.ChangeEmail(model);

            // Assert
            result.Model.Should().Be(model);

            _signInManagerMock.Verify(x => x.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            AssertZeroCommandsExecuted();
        }

        [Fact]
        public async Task ChangeEmail_Returns_ChangeEmail_View_When_User_Does_Not_Exist()
        {
            var model = new ChangeEmailViewModel();

            _userManagerMock.Setup(x => x.GetUserAsync(_controller.HttpContext.User))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = (ViewResult)await _controller.ChangeEmail(model);

            // Assert
            result.Model.Should().Be(model);

            _signInManagerMock.Verify(x => x.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            AssertZeroCommandsExecuted();
        }

        [Fact]
        public async Task ChangeEmail_Returns_View_With_Error_Notice_When_Password_Is_Incorrect()
        {
            var password = "password";
            var newEmail = "newEmail@gmail.com";
            var model = new ChangeEmailViewModel
            {
                NewEmail = newEmail,
                Password = password
            };

            var applicationUser = new ApplicationUser
            {
                UserName = "email@gmail.com"
            };

            _userManagerMock.Setup(x => x.GetUserAsync(_controller.HttpContext.User))
                .ReturnsAsync(applicationUser);

            _userManagerMock.Setup(x => x.CheckPasswordAsync(applicationUser, password))
                .ReturnsAsync(false);

            // Act
            var result = (ViewResult) await _controller.ChangeEmail(model);

            // Assert
            result.Model.Should().Be(model);

            _signInManagerMock.Verify(x => x.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            AssertZeroCommandsExecuted();

            var errorNotice = _controller.Notices.Dequeue();
            errorNotice.Text.Should().Be("Password is not correct!");
            errorNotice.Type.Should().Be(NoticeType.Error);
        }

        [Fact]
        public async Task ChangeEmail_Returns_View_With_Errors_When_ApplicationUser_Cant_Be_Updated()
        {
            var password = "password";
            var newEmail = "newEmail@gmail.com";
            var model = new ChangeEmailViewModel
            {
                NewEmail = newEmail,
                Password = password
            };

            var applicationUser = new ApplicationUser
            {
                UserName = "email@gmail.com"
            };

            _userManagerMock.Setup(x => x.GetUserAsync(_controller.HttpContext.User))
                .ReturnsAsync(applicationUser);

            _userManagerMock.Setup(x => x.CheckPasswordAsync(applicationUser, password))
                .ReturnsAsync(true);

            var errorDescription = "ErrorDescription";
            var identityErrorResult = IdentityResult.Failed(new IdentityError { Description = errorDescription });
            _userManagerMock.Setup(x => x.UpdateAsync(applicationUser))
                .ReturnsAsync(identityErrorResult);

            // Act
            var result = (ViewResult)await _controller.ChangeEmail(model);

            // Assert
            result.Model.Should().Be(model);

            _signInManagerMock.Verify(x => x.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
            AssertZeroCommandsExecuted();

            AssertModelStateContainsError(result, errorDescription);
        }

        [Fact]
        public async Task ChangeEmail_Returns_View_With_Error_Notice_When_Command_Was_Not_Executed_Successfulyy()
        {
            var password = "password";
            var newEmail = "newEmail@gmail.com";
            var model = new ChangeEmailViewModel
            {
                NewEmail = newEmail,
                Password = password
            };

            var applicationUser = new ApplicationUser
            {
                UserName = "email@gmail.com"
            };

            _userManagerMock.Setup(x => x.GetUserAsync(_controller.HttpContext.User))
                .ReturnsAsync(applicationUser);

            _userManagerMock.Setup(x => x.CheckPasswordAsync(applicationUser, password))
                .ReturnsAsync(true);

            var identitySuccessResult = IdentityResult.Success;
            _userManagerMock.Setup(x => x.UpdateAsync(applicationUser))
                .ReturnsAsync(identitySuccessResult);

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeUserEmailCommand>()))
                .Returns(CommandResult.Failed(""));

            // Act
            var result = (RedirectToActionResult)await _controller.ChangeEmail(model);

            // Assert
            result.ActionName.Should().Be("Index");

            var errorNotice = _controller.Notices.Dequeue();
            errorNotice.Text.Should().Be("Failed to change email!");
            errorNotice.Type.Should().Be(NoticeType.Error);
        }

        private static void AssertRedirectToIndex(RedirectToActionResult result)
        {
            result.ActionName.Should().Be(nameof(ManageController.Index));
        }
    }
}
