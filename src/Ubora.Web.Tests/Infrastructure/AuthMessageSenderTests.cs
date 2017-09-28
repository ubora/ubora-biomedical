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
using System.Collections.Generic;

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

            var uboraLogoContentId = "uboraLogoIcon";
            var emailLogoContentId = "emailLogoIcon";
            var facebookLogoContentId = "facebookLogoIcon";
            var twitterLogoContentId = "twitterLogoIcon";
            var emailIcons = new[]
            {
                new EmailIcon("./wwwroot/images/icon.png", uboraLogoContentId),
                new EmailIcon("./wwwroot/images/icons/email_icon.png", emailLogoContentId),
                new EmailIcon("./wwwroot/images/icons/facebook_icon.png", facebookLogoContentId),
                new EmailIcon("./wwwroot/images/icons/twitter_icon.png", twitterLogoContentId)
            };

            var callbackUrlTemplateViewModel = new CallBackUrlTemplateViewModel
            {
                Code = code,
                UserId = applicationUser.Id,
                EmailLogoContentId = emailLogoContentId,
                FacebookLogoContentId = facebookLogoContentId,
                TwitterLogoContentId = twitterLogoContentId,
                UboraLogoContentId = uboraLogoContentId
            };
            Expression<Func<CallBackUrlTemplateViewModel, bool>> expectedViewModelFunc = x => x.Code == callbackUrlTemplateViewModel.Code
                && x.UserId == callbackUrlTemplateViewModel.UserId
                && x.TwitterLogoContentId == callbackUrlTemplateViewModel.TwitterLogoContentId
                && x.UboraLogoContentId == callbackUrlTemplateViewModel.UboraLogoContentId
                && x.EmailLogoContentId == callbackUrlTemplateViewModel.EmailLogoContentId
                && x.FacebookLogoContentId == callbackUrlTemplateViewModel.FacebookLogoContentId;

            _viewRenderMock.Setup(r => r.Render("/_Features/_Shared/Templates/", "EmailConfirmationMessageTemplate.cshtml",
                It.Is(expectedViewModelFunc))).Returns(expectedMessage);

            Expression<Func<EmailIcon[], bool>> expectedEmailIconsFunc = b =>
                b[0].Path == emailIcons[0].Path && b[0].ContentId == emailIcons[0].ContentId
                && b[1].Path == emailIcons[1].Path && b[1].ContentId == emailIcons[1].ContentId
                && b[2].Path == emailIcons[2].Path && b[2].ContentId == emailIcons[2].ContentId
                && b[3].Path == emailIcons[3].Path && b[3].ContentId == emailIcons[3].ContentId;

            //Act
            await _sut.SendEmailConfirmationMessage(applicationUser);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmailAsync(applicationUser.Email, subject, expectedMessage, It.Is(expectedEmailIconsFunc)), Times.Once);
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

            var uboraLogoContentId = "uboraLogoIcon";
            var emailLogoContentId = "emailLogoIcon";
            var facebookLogoContentId = "facebookLogoIcon";
            var twitterLogoContentId = "twitterLogoIcon";
            var emailIcons = new[]
            {
                new EmailIcon("./wwwroot/images/icon.png", uboraLogoContentId),
                new EmailIcon("./wwwroot/images/icons/email_icon.png", emailLogoContentId),
                new EmailIcon("./wwwroot/images/icons/facebook_icon.png", facebookLogoContentId),
                new EmailIcon("./wwwroot/images/icons/twitter_icon.png", twitterLogoContentId)
            };

            var callbackUrlTemplateViewModel = new CallBackUrlTemplateViewModel
            {
                Code = code,
                UserId = applicationUser.Id,
                EmailLogoContentId = emailLogoContentId,
                FacebookLogoContentId = facebookLogoContentId,
                TwitterLogoContentId = twitterLogoContentId,
                UboraLogoContentId = uboraLogoContentId
            };
            Expression<Func<CallBackUrlTemplateViewModel, bool>> expectedViewModelFunc = x => x.Code == callbackUrlTemplateViewModel.Code
                && x.UserId == callbackUrlTemplateViewModel.UserId
                && x.TwitterLogoContentId == callbackUrlTemplateViewModel.TwitterLogoContentId
                && x.UboraLogoContentId == callbackUrlTemplateViewModel.UboraLogoContentId
                && x.EmailLogoContentId == callbackUrlTemplateViewModel.EmailLogoContentId
                && x.FacebookLogoContentId == callbackUrlTemplateViewModel.FacebookLogoContentId;

            _viewRenderMock.Setup(r => r.Render("/_Features/_Shared/Templates/", "ForgotPasswordMessageTemplate.cshtml", It.Is(expectedViewModelFunc)))
                .Returns(expectedMessage);

            Expression<Func<EmailIcon[], bool>> expectedEmailIconsFunc = b => 
                b[0].Path == emailIcons[0].Path && b[0].ContentId == emailIcons[0].ContentId
                && b[1].Path == emailIcons[1].Path && b[1].ContentId == emailIcons[1].ContentId
                && b[2].Path == emailIcons[2].Path && b[2].ContentId == emailIcons[2].ContentId
                && b[3].Path == emailIcons[3].Path && b[3].ContentId == emailIcons[3].ContentId;

            //Act
            await _sut.SendForgotPasswordMessage(applicationUser);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmailAsync(applicationUser.Email, subject, expectedMessage, It.Is(expectedEmailIconsFunc)), Times.Once);
        }
    }
}
