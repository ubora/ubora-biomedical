using System.Threading.Tasks;
using Moq;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure;
using Ubora.Web.Services;
using Ubora.Web.Tests.Fakes;
using Xunit;
using FluentAssertions;
using PreMailer.Net;
using Ubora.Web.Infrastructure.PreMailers;
using Ubora.Web._Features._Shared.Emails;

namespace Ubora.Web.Tests.Infrastructure
{
    public class ApplicationUserEmailMessageSenderTests
    {
        private readonly ApplicationUserEmailMessageSender _sut;
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<EmailSender> _emailSenderMock;
        private readonly Mock<ViewRender> _viewRenderMock;
        private readonly Mock<PreMailerFactory> _preMailerFactoryMock;

        public ApplicationUserEmailMessageSenderTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _emailSenderMock = new Mock<EmailSender>();
            _viewRenderMock = new Mock<ViewRender>();
            _preMailerFactoryMock = new Mock<PreMailerFactory>(); 
            _sut = new ApplicationUserEmailMessageSender(_userManagerMock.Object, _emailSenderMock.Object, _viewRenderMock.Object, _preMailerFactoryMock.Object);
        }

        [Fact]
        public async Task SendEmailConfirmationMessage_Test()
        {
            var user = new ApplicationUser
            {
                Email = "test@agileworks.eu"
            };

            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(user))
                .ReturnsAsync("expectedCode");

            EmailConfirmationEmailViewModel viewModel = null;
            _viewRenderMock.Setup(x => x.Render("/_Features/_Shared/Templates/", "EmailConfirmationMessageTemplate.cshtml", It.IsAny<EmailConfirmationEmailViewModel>()))
                .Returns("viewHtml")
                .Callback<string, string, EmailConfirmationEmailViewModel>((a, b, vm) => viewModel = vm);

            var preMailerMock = new Mock<PreMailerWrapper>();
            _preMailerFactoryMock.Setup(x => x.Create("viewHtml", null))
                .Returns(preMailerMock.Object);

            preMailerMock.Setup(x => x.MoveCssInline(true, ".ignore-premailer", null, false, false))
                .Returns(new InlineResult("finalHtml"));

            // Act
            await _sut.SendEmailConfirmationMessage(user);

            // Assert
            viewModel.UserId.Should().Be(user.Id);
            viewModel.Code.Should().Be("expectedCode");

            _emailSenderMock.Verify(x => x.SendEmailAsync("test@agileworks.eu", "UBORA: e-mail confirmation", "finalHtml", null, EmailLayoutViewModel.AddLayoutAttachments));
        }

        [Fact]
        public async Task SendForgotPasswordMessage_Test()
        {
            var user = new ApplicationUser
            {
                Email = "test@agileworks.eu"
            };

            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("expectedCode");

            ForgotPasswordEmailViewModel viewModel = null;
            _viewRenderMock.Setup(x => x.Render("/_Features/_Shared/Templates/", "ForgotPasswordMessageTemplate.cshtml", It.IsAny<ForgotPasswordEmailViewModel>()))
                .Returns("viewHtml")
                .Callback<string, string, ForgotPasswordEmailViewModel>((a, b, vm) => viewModel = vm);

            var preMailerMock = new Mock<PreMailerWrapper>();
            _preMailerFactoryMock.Setup(x => x.Create("viewHtml", null))
                .Returns(preMailerMock.Object);

            preMailerMock.Setup(x => x.MoveCssInline(true, ".ignore-premailer", null, false, false))
                .Returns(new InlineResult("finalHtml"));

            // Act
            await _sut.SendForgotPasswordMessage(user);

            // Assert
            viewModel.UserId.Should().Be(user.Id);
            viewModel.Code.Should().Be("expectedCode");

            _emailSenderMock.Verify(x => x.SendEmailAsync("test@agileworks.eu", "UBORA: Password reset", "finalHtml", null, EmailLayoutViewModel.AddLayoutAttachments));
        }
    }
}
