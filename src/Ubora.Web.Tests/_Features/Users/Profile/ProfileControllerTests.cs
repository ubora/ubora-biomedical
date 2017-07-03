using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Users.Profile;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.Profile
{
    public class ProfileControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<ICommandQueryProcessor> _commandQueryProcessorMock;
        private readonly Mock<IStorageProvider> _storageProviderMock;

        private readonly ProfileController _controller;

        public ProfileControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _commandQueryProcessorMock = new Mock<ICommandQueryProcessor>();
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _storageProviderMock = new Mock<IStorageProvider>();
            _controller = new ProfileController(_commandQueryProcessorMock.Object, _mapperMock.Object, _userManagerMock.Object, _signInManagerMock.Object, _storageProviderMock.Object);
            SetUserContext(_controller);
        }

        [Fact]
        public void EditProfile_Returns_View()
        {
            var userId = Guid.NewGuid().ToString();
            var userProfile = new UserProfile(new Guid(userId));
            var userProfileViewModel = new UserProfileViewModel();
            var model = new EditProfileViewModel()
            {
                UserViewModel = userProfileViewModel
            };

            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _mapperMock.Setup(m => m.Map<UserProfileViewModel>(It.IsAny<UserProfile>())).Returns(userProfileViewModel);

            _commandQueryProcessorMock.Setup(p => p.FindById<UserProfile>(It.IsAny<Guid>()))
                .Returns(userProfile);

            //Act
            var result = (ViewResult)_controller.EditProfile();

            //Assert
            result.ViewName.Should().Be("EditProfile");
            result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(model.UserViewModel);
        }

        [Fact]
        public async Task ChangeProfilePicture_Redirects_EditProfile_When_Command_Is_Executed_Successfully()
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();

            var model = new ProfilePictureViewModel
            {
                ProfilePicture = fileMock.Object,
                CurrentActionName = "testAction"
            };

            ChangeUserProfilePictureCommand executedCommand = null;
            var applicationUser = new ApplicationUser();

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _commandQueryProcessorMock.Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>())).Callback<ChangeUserProfilePictureCommand>(c => executedCommand = c)
                .Returns(new CommandResult());
            
            _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(applicationUser);

            //Act
            var result = (RedirectToActionResult)await _controller.ChangeProfilePicture(model);

            //Assert
            executedCommand.UserId.Should().Be(userId);
            result.ActionName.Should().Be(model.CurrentActionName);
            _signInManagerMock.Verify(m => m.RefreshSignInAsync(applicationUser),Times.Once);
        }

        [Fact]
        public async Task ChangeProfilePicture_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();

            var userProfile = new UserProfile(new Guid(userId));
            var userProfileViewModel = new UserProfileViewModel();

            var model = new ProfilePictureViewModel();
            model.ProfilePictureViewModel.ProfilePicture = fileMock.Object;
            model.UserViewModel = userProfileViewModel;

            var commandResult = new CommandResult("testError");

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            _commandQueryProcessorMock.Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>())).Returns(commandResult);

            _mapperMock.Setup(m => m.Map<UserProfileViewModel>(It.IsAny<UserProfile>())).Returns(userProfileViewModel);

            _commandQueryProcessorMock.Setup(p => p.FindById<UserProfile>(It.IsAny<Guid>()))
                .Returns(userProfile);

            //Act
            var result = (ViewResult)await _controller.ChangeProfilePicture(new ProfilePictureViewModel());

            //Assert
            result.ViewName.Should().Be("EditProfile");
            result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(model.UserViewModel);
            AssertModelStateContainsError(result, commandResult.ErrorMessages.Last());
            _userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()),Times.Never);
            _signInManagerMock.Verify(m => m.RefreshSignInAsync(It.IsAny<ApplicationUser>()),Times.Never);
        }

        [Fact]
        public async Task ChangeProfilePicture_View_With_ModelState_Errors_When_Validation_Result_Is_FailureAsync()
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();
            var userProfile = new UserProfile(new Guid(userId));
            var userViewModel = new UserProfileViewModel();

            var model = new EditProfileViewModel();
            model.UserViewModel = userViewModel;

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _mapperMock.Setup(m => m.Map<UserProfileViewModel>(It.IsAny<UserProfile>())).Returns(userViewModel);
            _commandQueryProcessorMock.Setup(p => p.FindById<UserProfile>(new Guid(userId)))
                .Returns(userProfile);

            _controller.ModelState.AddModelError("isImage", "This is not an image file");

            //Act
            var result = (ViewResult)await _controller.ChangeProfilePicture(new ProfilePictureViewModel());

            //Assert
            result.ViewName.Should().Be("EditProfile");
            result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(model.UserViewModel);
            AssertModelStateContainsError(result, "This is not an image file");
            _commandQueryProcessorMock.Verify(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>()), Times.Never);
            _userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()), Times.Never);
            _signInManagerMock.Verify(m => m.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
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
