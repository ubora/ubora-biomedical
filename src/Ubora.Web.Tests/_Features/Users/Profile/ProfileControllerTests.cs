using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Users.Profile;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.Profile
{
    public class ProfileControllerTests : UserControllerTestsBase
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<ICommandQueryProcessor> _commandQueryProcessorMock;
        private readonly Mock<IStorageProvider> _storageProviderMock;

        private readonly ProfileController _controller;

        public ProfileControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _commandQueryProcessorMock = new Mock<ICommandQueryProcessor>();
            _userManagerMock = new Mock<FakeUserManager>();
            _storageProviderMock = new Mock<IStorageProvider>();
            _controller = new ProfileController(_commandQueryProcessorMock.Object, _mapperMock.Object, _storageProviderMock.Object, _userManagerMock.Object);
            SetUserContext(_controller);
        }

        [Theory]
        [InlineData("/app/wwwroot/images/storages/profilePictures/ddfb5e55-7b9e-46a3-8d2c-b38bff50ca2edog.jpg")]
        [InlineData("Default")]
        public void EditProfile_Returns_View(string blobUrl)
        {
            var userId = Guid.NewGuid().ToString();
            var userProfile = new UserProfile(new Guid(userId));
            var userProfileViewModel = new UserViewModel();
            var model = new EditProfileViewModel()
            {
                UserViewModel = userProfileViewModel
            };

            _storageProviderMock.Setup(p => p.GetBlobUrl("profilePictures", It.IsAny<string>())).Returns("testUrl");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _mapperMock.Setup(m => m.Map<UserViewModel>(It.IsAny<UserProfile>())).Returns(userProfileViewModel);

            _commandQueryProcessorMock.Setup(p => p.FindById<UserProfile>(It.IsAny<Guid>()))
                .Returns(userProfile);

            //Act
            var result = (ViewResult)_controller.EditProfile();

            //Assert
            result.ViewName.Should().Be("EditProfile");
            result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(model.UserViewModel);
        }

        [Fact]
        public void ChangeProfilePicture_Redirects_EditProfile_When_Command_Is_Executed_Successfully()
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();

            var model = new EditProfileViewModel();
            model.ProfilePicture = fileMock.Object;

            ChangeUserProfilePictureCommand executedCommand = null;

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _commandQueryProcessorMock.Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>())).Callback<ChangeUserProfilePictureCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            //Act
            var result = (RedirectToActionResult)_controller.ChangeProfilePicture(model);

            //Assert
            executedCommand.UserId.Should().Be(userId);
            result.ActionName.Should().Be("EditProfile");
        }

        [Fact]
        public void ChangeProfilePicture_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();

            var userProfile = new UserProfile(new Guid(userId));
            var userProfileViewModel = new UserViewModel();

            var model = new EditProfileViewModel();
            model.ProfilePicture = fileMock.Object;
            model.UserViewModel = userProfileViewModel;

            var commandResult = new CommandResult("testError");

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            _commandQueryProcessorMock.Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>())).Returns(commandResult);

            _storageProviderMock.Setup(p => p.GetBlobUrl("profilePictures", It.IsAny<string>())).Returns("testUrl");
            _mapperMock.Setup(m => m.Map<UserViewModel>(It.IsAny<UserProfile>())).Returns(userProfileViewModel);

            _commandQueryProcessorMock.Setup(p => p.FindById<UserProfile>(It.IsAny<Guid>()))
                .Returns(userProfile);

            //Act
            var result = (ViewResult)_controller.ChangeProfilePicture(model);

            //Assert
            result.ViewName.Should().Be("EditProfile");
            result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(model.UserViewModel);
            AssertModelStateContainsError(result, commandResult.ErrorMessages.Last());
        }

        [Fact]
        public void ChangeProfilePicture_View_With_ModelState_Errors_When_Validation_Result_Is_Failure()
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();
            var userProfile = new UserProfile(new Guid(userId));
            var userViewModel = new UserViewModel();

            var model = new EditProfileViewModel();
            model.UserViewModel = userViewModel;

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _storageProviderMock.Setup(p => p.GetBlobUrl("profilePictures", It.IsAny<string>())).Returns("testUrl");
            _mapperMock.Setup(m => m.Map<UserViewModel>(It.IsAny<UserProfile>())).Returns(userViewModel);
            _commandQueryProcessorMock.Setup(p => p.FindById<UserProfile>(new Guid(userId)))
                .Returns(userProfile);

            _controller.ModelState.AddModelError("isImage", "This is not an image file");

            //Act
            var result = (ViewResult)_controller.ChangeProfilePicture(model);
            
            //Assert
            result.ViewName.Should().Be("EditProfile");
            result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(model.UserViewModel);
            AssertModelStateContainsError(result, "This is not an image file");
            _commandQueryProcessorMock.Verify(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>()), Times.Never);
        }

        [Fact]
        public void ViewProfile_Returns_View_And_ProfileViewModel_When_Should_Have_User()
        {
            var userId = Guid.NewGuid();
            var userprofile = new UserProfile(userId);
            var expectedProfileViewModel = new ProfileViewModel();

            _commandQueryProcessorMock.Setup(p => p.FindById<UserProfile>(userId))
                .Returns(userprofile);

            _mapperMock.Setup(m => m.Map<ProfileViewModel>(It.IsAny<UserProfile>()))
                .Returns(expectedProfileViewModel);

            //Act
            var result = (ViewResult)_controller.View(userId);

            //Act
            result.Model.As<ProfileViewModel>().Should().Be(expectedProfileViewModel);
        }

        [Fact]
        public void ViewProfile_Returns_NotFoundResult_If_Not_Found_User()
        {
            var userId = Guid.NewGuid();

            _commandQueryProcessorMock.Setup(p => p.FindById<UserProfile>(userId));

            //Act
            var result = _controller.View(userId);

            //Act
            result.Should().BeOfType<NotFoundResult>();
            _mapperMock.Verify(m => m.Map(It.IsAny<UserProfile>(), It.IsAny<ProfileViewModel>()), Times.Never);
        }
    }
}
