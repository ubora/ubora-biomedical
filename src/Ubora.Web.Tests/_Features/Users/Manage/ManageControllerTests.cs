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

namespace Ubora.Web.Tests._Features.Users.Manage
{
    public class ManageControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<EmailSender> _emailSenderMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<ILogger<ManageController>> _logger;
        private readonly ManageController _controller;

        public ManageControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _emailSenderMock = new Mock<EmailSender>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _logger = new Mock<ILogger<ManageController>>();

            _loggerFactoryMock.Setup(x => x.CreateLogger("ManageController"))
                .Returns(_logger.Object);
            
            _controller = new ManageController(_userManagerMock.Object, _signInManagerMock.Object, _emailSenderMock.Object, _loggerFactoryMock.Object);
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
            var result = (RedirectToActionResult) await _controller.ChangePassword(viewModel);

            // Assert
            _userManagerMock
                .Verify(x => x.ChangePasswordAsync(applicationUser, currentPassword, newPassword), Times.Once);

            var successNotice = _controller.Notices.Dequeue();
            successNotice.Text.Should().Be("Password changed successfully!");
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
                .ReturnsAsync((ApplicationUser) null);

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

        private static void AssertRedirectToIndex(RedirectToActionResult result)
        {
            result.ActionName.Should().Be(nameof(ManageController.Index));
        }
    }
}
