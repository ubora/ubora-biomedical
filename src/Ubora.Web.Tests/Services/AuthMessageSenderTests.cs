using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Web.Data;
using Ubora.Web.Services;
using Ubora.Web.Tests.Authorization;
using Ubora.Web.Tests.Fakes;
using Ubora.Web.Tests.Helper;
using Xunit;

namespace Ubora.Web.Tests.Services
{
    public class AuthMessageSenderTests
    {
        private readonly AuthMessageSender _sut;
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly Mock<IEmailSender> _emailSenderMock;

        public AuthMessageSenderTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _emailSenderMock = new Mock<IEmailSender>();
            _sut = new AuthMessageSender(_userManagerMock.Object, _urlHelperMock.Object, _emailSenderMock.Object);
        }

        [Fact]
        public async Task SendEmailConfirmationMessage_Sends_Confirmation_Message()
        {
            var applicationUser = new ApplicationUser { Email = "test@test.com", Id = Guid.NewGuid() };
            var code = "CfDJ8P0gmJ3q+8lOjMslRCyVLI489QEEOu2Pg/jBZf0eqbHkFI6CTBXS/Kj2gU9jrQW3LFe+3EPA0ez/bMdnz4Yj+951ShLZCJSLYc7ttMJ3/8bRr9/0GTNQ7Cxysuykp2/+/+rAW1TIbO7nDCK/X2BUf3PbaBUrdqaoszbsu/CnAQgVeDZBHJMGH1znLZw8R75WzmdaLJX/4a3xv2VZi0OElz70n4UiEs914Nn45aQ2zZXEV14OoLRRbZITgLeajRK8Rg==";
            var expectedUrl = $"http://ubora-dev.azurewebsites.net/Account/ConfirmEmail?userId=\"{applicationUser.Id}\"&code=\"{code}\"";
            var subject = "UBORA: e-mail confirmation";
            var expectedMessage = $"<h1 style='color:#4777BB; font-family: sans-serif; text-align:center;'>E-mail confirmation</h1><p>Please confirm your e-mail by clicking here on the following link or copy-paste it in your browser: <br /><a href=\"{expectedUrl}\">{expectedUrl}</a>.</p>";

            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(applicationUser))
                .ReturnsAsync(code);
            _urlHelperMock.Setup(h => h.ActionContext).Returns(new EmptyInitializedActionContext());

            UrlActionContext urlActionContext = null;
            _urlHelperMock
                .Setup(h => h.Action(It.IsAny<UrlActionContext>()))
                .Callback<UrlActionContext>(c => urlActionContext = c)
                .Returns(expectedUrl);

            //Act
            await _sut.SendEmailConfirmationMessage(applicationUser);

            //Assert
            urlActionContext.Action.Should().Be("ConfirmEmail");
            urlActionContext.Controller.Should().Be("Account");
            urlActionContext.Values.GetPropertyValue<Guid>("userId").Should().Be(applicationUser.Id);
            urlActionContext.Values.GetPropertyValue<string>("code").Should().Be(code);

            _emailSenderMock.Verify(x => x.SendEmailAsync(applicationUser.Email, subject, expectedMessage), Times.Once);
        }

        [Fact]
        public async Task SendForgotPasswordMessageAsync_Sends_Confirmation_Message()
        {
            var applicationUser = new ApplicationUser { Email = "test@test.com", Id = Guid.NewGuid() };
            var code = "CfDJ8P0gmJ3q+8lOjMslRCyVLI489QEEOu2Pg/jBZf0eqbHkFI6CTBXS/Kj2gU9jrQW3LFe+3EPA0ez/bMdnz4Yj+951ShLZCJSLYc7ttMJ3/8bRr9/0GTNQ7Cxysuykp2/+/+rAW1TIbO7nDCK/X2BUf3PbaBUrdqaoszbsu/CnAQgVeDZBHJMGH1znLZw8R75WzmdaLJX/4a3xv2VZi0OElz70n4UiEs914Nn45aQ2zZXEV14OoLRRbZITgLeajRK8Rg==";
            var expectedUrl = $"http://ubora-dev.azurewebsites.net/Account/ConfirmEmail?userId=\"{applicationUser.Id}\"&code=\"{code}\"";
            var subject = "UBORA: Password reset";
            var expectedMessage = $"<h1 style='color:#4777BB; font-family: sans-serif; text-align:center;'>Password reset</h1><p style='font-family:sans-serif;'>You can reset your password by clicking on the following link or copy-paste it in your browser: <br /><a href=\"{expectedUrl}\">{expectedUrl}</a>.</p>";

            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(applicationUser)).ReturnsAsync(code);
            _urlHelperMock
                .Setup(h => h.ActionContext)
                .Returns(new EmptyInitializedActionContext());

            UrlActionContext urlActionContext = null;
            _urlHelperMock
                .Setup(h => h.Action(It.IsAny<UrlActionContext>()))
                .Callback<UrlActionContext>(c => urlActionContext = c)
                .Returns(expectedUrl);

            //Act
            await _sut.SendForgotPasswordMessageAsync(applicationUser);

            //Assert
            urlActionContext.Action.Should().Be("ResetPassword");
            urlActionContext.Controller.Should().Be("Account");
            urlActionContext.Values.GetPropertyValue<Guid>("userId").Should().Be(applicationUser.Id);
            urlActionContext.Values.GetPropertyValue<string>("code").Should().Be(code);

            _emailSenderMock.Verify(x => x.SendEmailAsync(applicationUser.Email, subject, expectedMessage), Times.Once);
        }
    }
}
