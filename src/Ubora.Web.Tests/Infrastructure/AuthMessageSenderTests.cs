using System;
using System.Threading.Tasks;
using Moq;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure;
using Ubora.Web.Services;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features._Shared.Templates;
using Xunit;

namespace Ubora.Web.Tests.Infrastructure
{
    public class AuthMessageSenderTests
    {
        private readonly AuthMessageSender _sut;
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<IViewRender> _viewRenderMock;

        public AuthMessageSenderTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _emailSenderMock = new Mock<IEmailSender>();
            _viewRenderMock = new Mock<IViewRender>();
            _sut = new AuthMessageSender(_userManagerMock.Object, _emailSenderMock.Object, _viewRenderMock.Object);
        }

        [Fact]
        public async Task SendEmailConfirmationMessage_Sends_Confirmation_Message()
        {
            var applicationUser = new ApplicationUser { Email = "test@test.com", Id = Guid.NewGuid() };
            var code = "CfDJ8P0gmJ3q+8lOjMslRCyVLI489QEEOu2Pg/jBZf0eqbHkFI6CTBXS/Kj2gU9jrQW3LFe+3EPA0ez/bMdnz4Yj+951ShLZCJSLYc7ttMJ3/8bRr9/0GTNQ7Cxysuykp2/+/+rAW1TIbO7nDCK/X2BUf3PbaBUrdqaoszbsu/CnAQgVeDZBHJMGH1znLZw8R75WzmdaLJX/4a3xv2VZi0OElz70n4UiEs914Nn45aQ2zZXEV14OoLRRbZITgLeajRK8Rg==";
            var expectedUrl = $"http://ubora-dev.azurewebsites.net/Account/ConfirmEmail?userId=\"{applicationUser.Id}\"&code=\"{code}\"";
            var subject = "UBORA: e-mail confirmation";
            var expectedMessage = $"<h1 style='color:#4777BB;'>E-mail confirmation</h1><p>Please confirm your e-mail by clicking here or navigating to <a href=\"{expectedUrl}\">this link</a>.</p>";

            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(code);
            _viewRenderMock.Setup(r => r.Render("~/_Features/_Shared/Templates/", "EmailConfirmationMessageTemplate.cshtml",
                It.IsAny<CallBackUrlTemplateViewModel>())).Returns(expectedMessage);

            //Act
            await _sut.SendEmailConfirmationMessage(applicationUser);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmailAsync(applicationUser.Email, subject, expectedMessage), Times.Once);
        }

        [Fact]
        public async Task SendForgotPasswordMessageAsync_Sends_Confirmation_Message()
        {
            var applicationUser = new ApplicationUser { Email = "test@test.com", Id = Guid.NewGuid() };
            var code = "CfDJ8P0gmJ3q+8lOjMslRCyVLI489QEEOu2Pg/jBZf0eqbHkFI6CTBXS/Kj2gU9jrQW3LFe+3EPA0ez/bMdnz4Yj+951ShLZCJSLYc7ttMJ3/8bRr9/0GTNQ7Cxysuykp2/+/+rAW1TIbO7nDCK/X2BUf3PbaBUrdqaoszbsu/CnAQgVeDZBHJMGH1znLZw8R75WzmdaLJX/4a3xv2VZi0OElz70n4UiEs914Nn45aQ2zZXEV14OoLRRbZITgLeajRK8Rg==";
            var expectedUrl = $"http://ubora-dev.azurewebsites.net/Account/ConfirmEmail?userId=\"{applicationUser.Id}\"&code=\"{code}\"";
            var subject = "UBORA: Password reset";
            var expectedMessage = $"<h1 style='color:#4777BB;'>Password reset</h1><p>You can reset your password by clicking <a href=\"{expectedUrl}\">this link</a>.</p>";


            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(code);
            _viewRenderMock.Setup(r => r.Render("~/_Features/_Shared/Templates/", "ForgotPasswordMessageTemplate.cshtml",
                It.IsAny<CallBackUrlTemplateViewModel>())).Returns(expectedMessage);

            //Act
            await _sut.SendForgotPasswordMessage(applicationUser);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmailAsync(applicationUser.Email, subject, expectedMessage), Times.Once);
        }
    }
}
