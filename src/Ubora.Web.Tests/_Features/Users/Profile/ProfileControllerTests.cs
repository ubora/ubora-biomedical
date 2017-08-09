using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TwentyTwenty.Storage;
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
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<IStorageProvider> _storageProviderMock;

        private readonly ProfileController _controller;

        public ProfileControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _storageProviderMock = new Mock<IStorageProvider>();
            var urlHelperMock = new Mock<IUrlHelper>();
            _controller = new ProfileController(_userManagerMock.Object, _signInManagerMock.Object,
                _storageProviderMock.Object)
            {
                Url = urlHelperMock.Object
            };
            SetUpForTest(_controller);
        }

        [Fact]
        public void EditProfile_Returns_View()
        {
            var userProfile = new UserProfile(UserId);
            var userProfileViewModel = new UserProfileViewModel();
            var editProfileViewModel = new EditProfileViewModel
            {
                UserViewModel = userProfileViewModel
            };

            AutoMapperMock.Setup(m => m.Map<UserProfileViewModel>(userProfile))
                .Returns(userProfileViewModel);

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(UserId))
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

            var profilePictureViewModel = new ProfilePictureViewModel
            {
                ProfilePicture = fileMock.Object,
                IsFirstTimeEditProfile = isFirstTimeEditProfile
            };

            ChangeUserProfilePictureCommand executedCommand = null;
            var applicationUser = new ApplicationUser();

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(UserId.ToString);

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>()))
                .Callback<ChangeUserProfilePictureCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            _userManagerMock.Setup(m => m.FindByIdAsync(UserId.ToString())).ReturnsAsync(applicationUser);

            //Act
            var result = (RedirectToActionResult)await _controller.ChangeProfilePicture(profilePictureViewModel);

            //Assert
            result.ActionName.Should().Be(isFirstTimeEditProfile == false ? "EditProfile" : "FirstTimeEditProfile");
            executedCommand.UserId.Should().Be(UserId.ToString());
            _signInManagerMock.Verify(m => m.RefreshSignInAsync(applicationUser), Times.Once);
        }

        [Fact]
        public async Task ChangeProfilePicture_FirstTimeEditProfile_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();

            var userProfile = new UserProfile(UserId);
            var userProfileViewModel = new UserProfileViewModel();

            var profilePictureViewModel = new ProfilePictureViewModel
            {
                ProfilePicture = fileMock.Object,
                IsFirstTimeEditProfile = true
            };

            var commandResult = new CommandResult("testError");

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(UserId.ToString);

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>()))
                .Returns(commandResult);

            AutoMapperMock.Setup(m => m.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(UserId))
                .Returns(userProfile);

            //Act
            var result = (ViewResult)await _controller.ChangeProfilePicture(profilePictureViewModel);

            //Assert
            result.ViewName.Should().Be("FirstTimeEditProfile");
            result.Model.As<FirstTimeEditProfileModel>().ProfilePictureViewModel.IsFirstTimeEditProfile
                .Should().BeTrue();
        }

        [Fact]
        public async Task ChangeProfilePicture_EditProfile_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();

            var userProfile = new UserProfile(UserId);
            var userProfileViewModel = new UserProfileViewModel();

            var profilePictureViewModel = new ProfilePictureViewModel
            {
                ProfilePicture = fileMock.Object,
                IsFirstTimeEditProfile = false
            };

            var commandResult = new CommandResult("testError");

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(UserId.ToString);

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>()))
                .Returns(commandResult);

            AutoMapperMock.Setup(m => m.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(UserId))
                .Returns(userProfile);

            //Act
            var result = (ViewResult)await _controller.ChangeProfilePicture(profilePictureViewModel);

            //Assert
            result.ViewName.Should().Be("EditProfile");
            result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(userProfileViewModel);
            AssertModelStateContainsError(result, commandResult.ErrorMessages.Last());
            _userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()), Times.Never);
            _signInManagerMock.Verify(m => m.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task ChangeProfilePicture_Returns_FirstTimeEditProfile_View_With_ModelState_Errors_When_Validation_Result_Is_Failure()
        {
            var fileMock = new Mock<IFormFile>();
            var userProfile = new UserProfile(UserId);
            var userViewModel = new UserProfileViewModel();

            var profilePictureViewModel = new ProfilePictureViewModel()
            {
                IsFirstTimeEditProfile = true
            };

            fileMock
                .Setup(f => f.FileName)
                .Returns("C:\\Test\\Parent\\Parent\\image.png");

            AutoMapperMock.Setup(m => m.Map<UserProfileViewModel>(userProfile))
                .Returns(userViewModel);

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(UserId))
                .Returns(userProfile);

            _controller.ModelState.AddModelError("isImage", "This is not an image file");

            //Act
            var result = (ViewResult)await _controller.ChangeProfilePicture(profilePictureViewModel);

            //Assert
            result.ViewName.Should().Be("FirstTimeEditProfile");

            result.Model.As<FirstTimeEditProfileModel>().ProfilePictureViewModel.IsFirstTimeEditProfile
                .Should().BeTrue();
        }

        [Fact]
        public async Task ChangeProfilePicture_Returns_EditProfile_View_With_ModelState_Errors_When_Validation_Result_Is_Failure()
        {
            var fileMock = new Mock<IFormFile>();
            var userProfile = new UserProfile(UserId);
            var userViewModel = new UserProfileViewModel();

            var profilePictureViewModel = new ProfilePictureViewModel()
            {
                IsFirstTimeEditProfile = false
            };

            fileMock
                .Setup(f => f.FileName)
                .Returns("C:\\Test\\Parent\\Parent\\image.png");

            AutoMapperMock.Setup(m => m.Map<UserProfileViewModel>(userProfile))
                .Returns(userViewModel);

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(UserId))
                .Returns(userProfile);

            _controller.ModelState.AddModelError("isImage", "This is not an image file");

            //Act
            var result = (ViewResult)await _controller.ChangeProfilePicture(profilePictureViewModel);

            //Assert
            result.ViewName.Should().Be("EditProfile");
            result.Model.As<EditProfileViewModel>().UserViewModel.Should().Be(userViewModel);
            AssertModelStateContainsError(result, "This is not an image file");

            CommandProcessorMock.Verify(p => p.Execute(It.IsAny<ChangeUserProfilePictureCommand>()),
                Times.Never);

            _userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()), Times.Never);

            _signInManagerMock.Verify(m => m.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public void View_Returns_View_And_ProfileViewModel_When_User_Exists()
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

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(userId)).Returns((UserProfile)null);

            //Act
            var result = _controller.View(userId);

            //Act
            result.Should().BeOfType<NotFoundResult>();
            AutoMapperMock.Verify(m => m.Map(It.IsAny<UserProfile>(), It.IsAny<ProfileViewModel>()), Times.Never);
        }

        [Fact]
        public void FirstTimeEditProfile_Returns_View()
        {
            //Act
            var result = (ViewResult)_controller.FirstTimeEditProfile();

            //Assert
            result.ViewName.Should().Be("FirstTimeEditProfile");
            result.Model.As<FirstTimeEditProfileModel>().ProfilePictureViewModel.IsFirstTimeEditProfile.Should()
                .BeTrue();
        }

        [Fact]
        public void FirstTimeEditProfile_Returns_FirstTimeEditProfile_View_When_User_Has_Not_Edited_Profile_First_Time()
        {
            //Act
            var result = (ViewResult)_controller.FirstTimeEditProfile();

            //Assert
            result.ViewName.Should().Be("FirstTimeEditProfile");
            result.Model.As<FirstTimeEditProfileModel>().ProfilePictureViewModel.IsFirstTimeEditProfile.Should()
                .BeTrue();
        }

        [Fact]
        public void FirstTimeEditProfile_Redirects_ReturnUrl_When_Command_Is_Executed_Successfully()
        {
            EditUserProfileCommand executedCommand = null;

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<EditUserProfileCommand>()))
                .Callback<EditUserProfileCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            var userProfile = new UserProfile(UserId) { FirstName = "expactedFirstName", LastName = "expactedLastName" };

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(It.IsAny<Guid>()))
                .Returns(userProfile);

            //Act
            var result =
                (RedirectToActionResult)_controller.FirstTimeEditProfile(new FirstTimeUserProfileViewModel());

            //Assert
            result.ActionName.Should().Be("Index");
            executedCommand.UserId.Should().Be(UserId);
        }

        [Fact]
        public void FirstTimeEditProfile_Returns_View_With_ModelState_Errors_When_Validation_Result_Is_Failure()
        {
            var errorMessage = "errormessage";
            _controller.ModelState.AddModelError("errorKey", errorMessage);

            //Act
            var result = (ViewResult)_controller.FirstTimeEditProfile(new FirstTimeUserProfileViewModel());

            //Assert
            result.ViewName.Should().Be("FirstTimeEditProfile");
            result.Model.As<FirstTimeEditProfileModel>().ProfilePictureViewModel.IsFirstTimeEditProfile.Should()
                .BeTrue();
            AssertModelStateContainsError(result, errorMessage);
        }

        [Fact]
        public void FirstTimeEditProfile_Returns_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var commandResult = new CommandResult("testError");

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<EditUserProfileCommand>()))
                .Returns(commandResult);

            var userProfile = new UserProfile(UserId) { FirstName = "expactedFirstName", LastName = "expactedLastName" };

            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(UserId))
                .Returns(userProfile);

            //Act
            var result = (ViewResult)_controller.FirstTimeEditProfile(new FirstTimeUserProfileViewModel());

            //Assert
            result.ViewName.Should().Be("FirstTimeEditProfile");
            result.Model.As<FirstTimeEditProfileModel>().ProfilePictureViewModel.IsFirstTimeEditProfile.Should()
                .BeTrue();
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }
    }
}
