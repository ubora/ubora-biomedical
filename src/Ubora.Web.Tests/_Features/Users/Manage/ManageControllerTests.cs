using System;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Web.Services;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Users.Manage;
using Ubora.Web._Features._Shared;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.Manage
{
    public class ManageControllerTests : UserControllerTestsBase
    {
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<IOptions<IdentityCookieOptions>> _identityCookieOptionsMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<ISmsSender> _smsSenderMock;
        private readonly Mock<ICommandQueryProcessor> _commandProcessorMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<IStorageProvider> _storageProviderMock;
        private readonly Mock<IManageValidator> _manageValidatorMock;
        private readonly Mock<IModelStateUpdater> _modelStateUpdaterMock;
        private readonly ManageController _controller;

        public ManageControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _identityCookieOptionsMock = new Mock<IOptions<IdentityCookieOptions>>();
            _identityCookieOptionsMock.Setup(o => o.Value).Returns(new IdentityCookieOptions());
            _emailSenderMock = new Mock<IEmailSender>();
            _smsSenderMock = new Mock<ISmsSender>();
            _commandProcessorMock = new Mock<ICommandQueryProcessor>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _storageProviderMock = new Mock<IStorageProvider>();
            _manageValidatorMock = new Mock<IManageValidator>();
            _modelStateUpdaterMock = new Mock<IModelStateUpdater>();

            _controller = new ManageController(_userManagerMock.Object, _signInManagerMock.Object,
                _identityCookieOptionsMock.Object, _emailSenderMock.Object, _smsSenderMock.Object,
                _loggerFactoryMock.Object, _commandProcessorMock.Object, _storageProviderMock.Object, _manageValidatorMock.Object, _modelStateUpdaterMock.Object);
            SetManageAndUserContext(_controller);
        }

        [Fact]
        public void ChangeProfilePicture_Redirects_EditProfile_When_Command_Is_Executed_Successfully()
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();

            var validationResult = new ValidationResult();

            ChangeUserProfilePictureCommand executedCommand = null;

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _commandProcessorMock.Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>())).Callback<ChangeUserProfilePictureCommand>(c => executedCommand = c)
                .Returns(new CommandResult());
            _manageValidatorMock.Setup(v => v.IsImage(It.IsAny<IFormFile>())).Returns(validationResult);

            //Act
            var result = (RedirectToActionResult) _controller.ChangeProfilePicture(fileMock.Object);

            //Assert
            executedCommand.UserId.Should().Be(userId);
            result.ActionName.Should().Be("EditProfile");
        }

        [Fact]
        public void ChangeProfilePicture_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();

            var validationResult = new ValidationResult();

            ChangeUserProfilePictureCommand executedCommand = null;

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _commandProcessorMock.Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>())).Returns(new CommandResult("testError"));
            _manageValidatorMock.Setup(v => v.IsImage(It.IsAny<IFormFile>())).Returns(validationResult);

            //Act
            var result = (RedirectToActionResult)_controller.ChangeProfilePicture(fileMock.Object);

            //Assert
            executedCommand.UserId.Should().Be(userId);
            result.ActionName.Should().Be("EditProfile");
        }
    }
}
