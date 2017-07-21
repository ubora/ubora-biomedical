using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Users.Profile;
using Xunit;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web.Tests._Features.Users.Profile
{
    public class ProfileControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<ImageStorageProvider> _imageStorageProviderMock;

        private readonly ProfileController _controller;

        public ProfileControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _imageStorageProviderMock = new Mock<ImageStorageProvider>();
            _controller = new ProfileController(_userManagerMock.Object, _signInManagerMock.Object, _imageStorageProviderMock.Object);
            SetUpForTest(_controller);
        }

        [Fact]
        public void EditProfile_Returns_View()
        {
            var userId = Guid.NewGuid().ToString();
            var userProfile = new UserProfile(new Guid(userId));
            var userProfileViewModel = new UserProfileViewModel();
            var editProfileViewModel = new EditProfileViewModel()
            {
                UserViewModel = userProfileViewModel
            };

            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            AutoMapperMock.Setup(m => m.Map<UserProfileViewModel>(userProfile))
                .Returns(userProfileViewModel);

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(new Guid(userId)))
                .Returns(userProfile);

            //Act
            var result = (ViewResult)_controller.EditProfile();

            //Assert
            result.ViewName.Should().Be("EditProfile");
            result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(editProfileViewModel.UserViewModel);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ChangeProfilePicture_Redirects_EditProfile_When_Command_Is_Executed_Successfully(bool isFirstTimeEditProfile)
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();

            var profilePictureViewModel = new ProfilePictureViewModel
            {
                ProfilePicture = fileMock.Object,
                IsFirstTimeEditProfile = isFirstTimeEditProfile
            };

            ChangeUserProfilePictureCommand executedCommand = null;
            var applicationUser = new ApplicationUser();

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>()))
                .Callback<ChangeUserProfilePictureCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(applicationUser);

            //Act
            var result = (RedirectToActionResult)await _controller.ChangeProfilePicture(profilePictureViewModel);

            //Assert
            if (isFirstTimeEditProfile == false)
            {
                result.ActionName.Should().Be("EditProfile");
            }
            else
            {
                result.ActionName.Should().Be("FirstTimeEditProfile");
            }
            executedCommand.UserId.Should().Be(userId);
            _signInManagerMock.Verify(m => m.RefreshSignInAsync(applicationUser), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ChangeProfilePicture_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful(
            bool isFirstTimeEditProfile)
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();

            var userProfile = new UserProfile(new Guid(userId));
            var userProfileViewModel = new UserProfileViewModel();

            var profilePictureViewModel = new ProfilePictureViewModel()
            {
                ProfilePicture = fileMock.Object,
                IsFirstTimeEditProfile = isFirstTimeEditProfile
            };

            var commandResult = new CommandResult("testError");

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>()))
                .Returns(commandResult);

            AutoMapperMock.Setup(m => m.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(new Guid(userId)))
                .Returns(userProfile);

            //Act
            var result = (ViewResult)await _controller.ChangeProfilePicture(profilePictureViewModel);

            //Assert
            if (isFirstTimeEditProfile == false)
            {
                result.ViewName.Should().Be("EditProfile");
                result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(userProfileViewModel);
                AssertModelStateContainsError(result, commandResult.ErrorMessages.Last());
                _userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()), Times.Never);
                _signInManagerMock.Verify(m => m.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
            }
            else
            {
                result.ViewName.Should().Be("FirstTimeEditProfile");
                result.Model.As<FirstTimeEditProfileModel>().ProfilePictureViewModel.IsFirstTimeEditProfile
                    .Should().Be(isFirstTimeEditProfile);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ChangeProfilePicture_View_With_ModelState_Errors_When_Validation_Result_Is_FailureAsync(bool isFirstTimeEditProfile)
        {
            var fileMock = new Mock<IFormFile>();
            var userId = Guid.NewGuid().ToString();
            var userProfile = new UserProfile(new Guid(userId));
            var userViewModel = new UserProfileViewModel();

            var profilePictureViewModel = new ProfilePictureViewModel()
            {
                IsFirstTimeEditProfile = isFirstTimeEditProfile
            };

            fileMock
                .Setup(f => f.FileName)
                .Returns("C:\\Test\\Parent\\Parent\\image.png");

            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            AutoMapperMock.Setup(m => m.Map<UserProfileViewModel>(userProfile))
                .Returns(userViewModel);

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(new Guid(userId)))
                .Returns(userProfile);

            _controller.ModelState.AddModelError("isImage", "This is not an image file");

            //Act
            var result = (ViewResult)await _controller.ChangeProfilePicture(profilePictureViewModel);

            //Assert
            if (isFirstTimeEditProfile == false)
            {
                result.ViewName.Should().Be("EditProfile");
                result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(userViewModel);
                AssertModelStateContainsError(result, "This is not an image file");

                CommandProcessorMock.Verify(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>()),
                    Times.Never);

                _userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()), Times.Never);

                _signInManagerMock.Verify(m => m.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
            }
            else
            {
                result.ViewName.Should().Be("FirstTimeEditProfile");

                result.Model.As<FirstTimeEditProfileModel>().ProfilePictureViewModel.IsFirstTimeEditProfile
                    .Should().Be(isFirstTimeEditProfile);
            }

        }

        [Fact]
        public void ViewProfile_Returns_View_And_ProfileViewModel_When_Should_Have_User()
        {
            var userId = Guid.NewGuid();
            var userprofile = new UserProfile(userId);
            var expectedProfileViewModel = new ProfileViewModel();

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(userId))
                .Returns(userprofile);

            AutoMapperMock.Setup(m => m.Map<ProfileViewModel>(userprofile))
                .Returns(expectedProfileViewModel);

            //Act
            var result = (ViewResult)_controller.View(userId);

            //Act
            result.Model.As<ProfileViewModel>()
                .Should().Be(expectedProfileViewModel);
        }

        [Fact]
        public void ViewProfile_Returns_NotFoundResult_If_Not_Found_User()
        {
            var userId = Guid.NewGuid();

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(userId));

            //Act
            var result = _controller.View(userId);

            //Act
            result.Should().BeOfType<NotFoundResult>();
            AutoMapperMock.Verify(m => m.Map(It.IsAny<UserProfile>(), It.IsAny<ProfileViewModel>()), Times.Never);
        }
    }
}
