using System;
using System.Threading.Tasks;
using Moq;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure;
using Ubora.Web.Services;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features._Shared.Templates;
using Xunit;
using System.Linq.Expressions;

namespace Ubora.Web.Tests.Infrastructure
{
    public class AuthMessageSenderTests
    {
        private readonly AuthMessageSender _sut;
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<ViewRender> _viewRenderMock;

        public AuthMessageSenderTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _emailSenderMock = new Mock<IEmailSender>();
            _viewRenderMock = new Mock<ViewRender>();
            _sut = new AuthMessageSender(_userManagerMock.Object, _emailSenderMock.Object, _viewRenderMock.Object);
        }

        [Fact]
        public async Task SendEmailConfirmationMessage_Sends_Confirmation_Message()
        {
            var applicationUser = new ApplicationUser
            {
                Email = "email",
                Id = Guid.NewGuid()
            };

            var code = "confimationCode";
            var subject = "UBORA: e-mail confirmation";
            var expectedMessage = "expectedMessage";

            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(applicationUser))
                .ReturnsAsync(code);

            var callbackUrlTemplateViewModel = new CallBackUrlTemplateViewModel
            {
                Code = code,
                UserId = applicationUser.Id
            };
            Expression<Func<CallBackUrlTemplateViewModel, bool>> expectedViewModelFunc = x => x.Code == callbackUrlTemplateViewModel.Code
                && x.UserId == callbackUrlTemplateViewModel.UserId;

            _viewRenderMock.Setup(r => r.Render("/_Features/_Shared/Templates/", "EmailConfirmationMessageTemplate.cshtml",
                It.Is(expectedViewModelFunc))).Returns(expectedMessage);

            //Act
            await _sut.SendEmailConfirmationMessage(applicationUser);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmailAsync(applicationUser.Email, subject, expectedMessage), Times.Once);
        }

        [Fact]
        public async Task SendForgotPasswordMessageAsync_Sends_Confirmation_Message()
        {
            var applicationUser = new ApplicationUser
            {
                Email = "email",
                Id = Guid.NewGuid()
            };
            var code = "confirmationCode";
            var subject = "UBORA: Password reset";
            var expectedMessage = $"expectedMessage";

            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(applicationUser)).ReturnsAsync(code);

            var callbackUrlTemplateViewModel = new CallBackUrlTemplateViewModel
            {
                Code = code,
                UserId = applicationUser.Id
            };
            Expression<Func<CallBackUrlTemplateViewModel, bool>> expectedViewModelFunc = x => x.Code == callbackUrlTemplateViewModel.Code
                && x.UserId == callbackUrlTemplateViewModel.UserId;

            _viewRenderMock.Setup(r => r.Render("/_Features/_Shared/Templates/", "ForgotPasswordMessageTemplate.cshtml", It.Is(expectedViewModelFunc)))
                .Returns(expectedMessage);

            //Act
            await _sut.SendForgotPasswordMessage(applicationUser);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmailAsync(applicationUser.Email, subject, expectedMessage), Times.Once);
        }
    }
}
