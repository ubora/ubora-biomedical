﻿using Microsoft.AspNetCore.Identity;
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
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users.Commands;
using System;

namespace Ubora.Web.Tests._Features.Users.Manage
{
    public class ManageControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<EmailSender> _emailSenderMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<ILogger<ManageController>> _logger;
        private readonly Mock<IEmailChangeMessageSender> _emailChangeMessageSenderMock;
        private readonly ManageController _controller;

        public ManageControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _emailSenderMock = new Mock<EmailSender>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _logger = new Mock<ILogger<ManageController>>();
            _emailChangeMessageSenderMock = new Mock<IEmailChangeMessageSender>();

            _loggerFactoryMock.Setup(x => x.CreateLogger("ManageController"))
                .Returns(_logger.Object);

            _controller = new ManageController(_userManagerMock.Object, _signInManagerMock.Object, _emailSenderMock.Object, _loggerFactoryMock.Object, _emailChangeMessageSenderMock.Object);
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


            // Act
            var result = (RedirectToActionResult)await _controller.ChangePassword(viewModel);

            // Assert
            _userManagerMock
                .Verify(x => x.ChangePasswordAsync(It.IsAny<ApplicationUser>(), currentPassword, newPassword), Times.Never);

            _signInManagerMock
               .Verify(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null), Times.Never);

            var successNotice = _controller.Notices.Dequeue();
            successNotice.Text.Should().Contain("Password could not be changed");
            successNotice.Type.Should().Be(NoticeType.Error);

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
            _userManagerMock.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            AssertZeroCommandsExecuted();
        }

        [Fact]
        public async Task ChangeEmail_Returns_ChangeEmail_ViewWith_Error_Notice_When_Email_Is_Already_Taken()
        {
            var model = new ChangeEmailViewModel();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            // Act
            var result = (ViewResult)await _controller.ChangeEmail(model);

            // Assert
            _signInManagerMock.Verify(x => x.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            AssertZeroCommandsExecuted();

            _controller.ModelState[nameof(model.NewEmail)].Errors[0].ErrorMessage.Should().Contain("Email is already taken");
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
            var result = (ViewResult)await _controller.ChangeEmail(model);

            // Assert
            _signInManagerMock.Verify(x => x.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            AssertZeroCommandsExecuted();

            _controller.ModelState[nameof(model.Password)].Errors[0].ErrorMessage.Should().Contain("Password is not correct");
        }

        [Fact]
        public async Task ConfirmChangeEmail_Redirects_To_Home_Index_With_Error_Notice_When_When_Token_Isnt_valid()
        {
            var userId = Guid.NewGuid().ToString();
            var code = "testCode";
            var newEmail = "new@test.com";

            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());

            //Act
            var result = (RedirectToActionResult)await _controller.ConfirmChangeEmail(userId, code, newEmail);

            //Assert
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");

            var errorNotice = _controller.Notices.Dequeue();
            errorNotice.Text.Should().Be("Confirmation code is wrong or expired");
            errorNotice.Type.Should().Be(NoticeType.Error);

            AssertZeroCommandsExecuted();
            _emailChangeMessageSenderMock.Verify(x => x.SendEmailChangeConfirmationMessage(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmChangeEmail_Redirects_To_Home_Index_With_Error_Notice_When_ApplicationUser_Cant_Be_Updated()
        {
            var userId = Guid.NewGuid().ToString();
            var code = "testCode";
            var newEmail = "new@test.com";

            var applicationUser = new ApplicationUser();
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.VerifyUserTokenAsync(applicationUser, "Default", "ChangeEmail", code))
                .ReturnsAsync(true);
            var errorDescription = "ErrorDescription";
            var identityErrorResult = IdentityResult.Failed(new IdentityError { Description = errorDescription });
            _userManagerMock.Setup(x => x.UpdateAsync(applicationUser))
                .ReturnsAsync(identityErrorResult);

            //Act
            var result = (RedirectToActionResult)await _controller.ConfirmChangeEmail(userId, code, newEmail);

            //Assert
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");

            var errorNotice = _controller.Notices.Dequeue();
            errorNotice.Text.Should().Be($"Email could not be changed. Reason: {errorDescription}");
            errorNotice.Type.Should().Be(NoticeType.Error);

            AssertZeroCommandsExecuted();
            _emailChangeMessageSenderMock.Verify(x => x.SendEmailChangeConfirmationMessage(applicationUser, It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmChangeEmail_Redirects_To_Home_Index_With_Error_Notice_When_Command_Was_Not_Executed_Successfulyy()
        {
            var userId = Guid.NewGuid().ToString();
            var code = "testCode";
            var newEmail = "new@test.com";

            var applicationUser = new ApplicationUser();
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.VerifyUserTokenAsync(applicationUser, "Default", "ChangeEmail", code))
                .ReturnsAsync(true);

            var identitySuccessResult = IdentityResult.Success;
            _userManagerMock.Setup(x => x.UpdateAsync(applicationUser))
                .ReturnsAsync(identitySuccessResult);

            var errorMessage = "NullPointerException";

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeUserEmailCommand>()))
                .Returns(CommandResult.Failed(errorMessage));

            //Act
            var result = (RedirectToActionResult)await _controller.ConfirmChangeEmail(userId, code, newEmail);

            //Assert
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");

            var errorNotice = _controller.Notices.Dequeue();
            errorNotice.Text.Should().Be(errorMessage);
            errorNotice.Type.Should().Be(NoticeType.Error);

            _emailChangeMessageSenderMock.Verify(x => x.SendEmailChangeConfirmationMessage(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmChangeEmail_Redirects_To_Home_Index_Notice_When_Command_Was_Executed_Successfulyy()
        {
            var userId = Guid.NewGuid().ToString();
            var code = "testCode";
            var newEmail = "new@test.com";
            var oldEmail = "old@test.com";

            var applicationUser = new ApplicationUser();
            applicationUser.Email = oldEmail;
            applicationUser.UserName = oldEmail;
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.VerifyUserTokenAsync(applicationUser, "Default", "ChangeEmail", code))
                .ReturnsAsync(true);

            var identitySuccessResult = IdentityResult.Success;
            _userManagerMock.Setup(x => x.UpdateAsync(applicationUser))
                .ReturnsAsync(identitySuccessResult);

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeUserEmailCommand>()))
                .Returns(CommandResult.Success);

            //Act
            var result = (RedirectToActionResult)await _controller.ConfirmChangeEmail(userId, code, newEmail);

            //Assert
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");

            _emailChangeMessageSenderMock.Verify(x => x.SendChangedEmailMessage(applicationUser, oldEmail), Times.Once);
        }

        private static void AssertRedirectToIndex(RedirectToActionResult result)
        {
            result.ActionName.Should().Be(nameof(ManageController.Index));
        }
    }
}
