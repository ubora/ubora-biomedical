using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
        public async Task SendForgotPasswordMessageAsync_Sends_Confirmation_Message()
        {
            var applicationUser = new ApplicationUser() { Email = "test@test.com" };
            var emailConfirmationToken = "342hdba7ydi3di73h2hia7d7i3";
            var expectedUrl = "https://www.google.com/";
            var resetPassword = "Password reset";
            var expectedMessage = $"<h1 style='color:#4777BB;'>Password reset</h1><p>You can reset your password by clicking <a href=\"{expectedUrl}\">this link</a>.</p>";

            _userManagerMock.Setup(
                    x => x.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(emailConfirmationToken);

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
            urlActionContext.Values.GetPropertyValue<string>("code").Should().Be(emailConfirmationToken);

            _emailSenderMock.Verify(x => x.SendEmailAsync(applicationUser.Email, resetPassword, expectedMessage), Times.Once);
        }
    }
}
