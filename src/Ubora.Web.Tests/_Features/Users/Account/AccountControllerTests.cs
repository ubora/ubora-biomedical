using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Services;
using Ubora.Web.Tests.Fakes;
using Ubora.Web.Tests.Helper;
using Ubora.Web._Features.Home;
using Ubora.Web._Features.Users.Account;
using Ubora.Web._Features.Users.Profile;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Ubora.Web.Tests._Features.Users.Account
{
    public class AccountControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<IOptions<IdentityCookieOptions>> _identityCookieOptionsMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<ILogger<AccountController>> _loggerMock;
        private readonly Mock<IAuthMessageSender> _authMessageSenderMock;
        private readonly AccountController _controller;
        public const string UserId = "testUserId";
        public const string Code = "testCode";

        public AccountControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _identityCookieOptionsMock = new Mock<IOptions<IdentityCookieOptions>>();
            _identityCookieOptionsMock.Setup(o => o.Value).Returns(new IdentityCookieOptions());
            _emailSenderMock = new Mock<IEmailSender>();
            _loggerMock = new Mock<ILogger<AccountController>>();
            _authMessageSenderMock = new Mock<IAuthMessageSender>(MockBehavior.Strict);
            var urlHelperMock = new Mock<IUrlHelper>();
            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object,
                _identityCookieOptionsMock.Object, _emailSenderMock.Object,
                _loggerMock.Object, _authMessageSenderMock.Object)
            {
                Url = urlHelperMock.Object
            };
            SetUpForTest(_controller);
        }

        [Fact]
        public async void Login_Returns_View_And_Model_When_ModelState_Is_Invalid()
        {
            var loginviewmodel = new LoginViewModel
            {
                Email = "test@test.com",
                Password = "testLoginViewModelPassword"
            };

            const string errorMessage = "mock error message";
            _controller.ViewData.ModelState.AddModelError("", errorMessage);

            //Act
            var result = await _controller.Login(loginviewmodel, "/") as ViewResult;
            //Assert
            result.Model.Should().Be(loginviewmodel);
            result.ViewData.Values.Last().Should().Be("/");
            AssertModelStateContainsError(result, errorMessage);

            _userManagerMock.Verify(m => m.FindByNameAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(m => m.IsEmailConfirmedAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
            QueryProcessorMock.Verify(p => p.FindById<UserProfile>(It.IsAny<Guid>()),Times.Never);
        }

        [Fact]
        public async void Login_Returns_View_With_Error_If_Is_Not_Email_Confirmed()
        {
            var loginviewmodel = GetLoginViewModel();

            var applicationUser = new ApplicationUser();
            _userManagerMock.Setup(x => x.FindByNameAsync(loginviewmodel.Email)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(applicationUser)).ReturnsAsync(false);

            //Act
            var result = await _controller.Login(loginviewmodel, "/") as ViewResult;

            //Assert  
            result.Model.Should().Be(loginviewmodel);
            result.ViewData.Values.Last().Should().Be("/");
            AssertModelStateContainsError(result, "You must have a confirmed email to log in.");

            _signInManagerMock.Verify(
                m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Never);
            QueryProcessorMock.Verify(p => p.FindById<UserProfile>(It.IsAny<Guid>()), Times.Never);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void Login_Signs_In_Manager_And_Redirects_To_ReturnUrl_When_Are_Success(bool isFirstTimeEditedProfile)
        {
            var loginviewmodel = GetLoginViewModel();

            var signInResult = new SignInResult();
            signInResult.Set(nameof(SignInResult.Succeeded), true);

            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid()
            };
            _userManagerMock.Setup(x => x.FindByNameAsync(loginviewmodel.Email)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(applicationUser)).ReturnsAsync(true);
            _signInManagerMock.Setup(
                    x => x.PasswordSignInAsync(loginviewmodel.Email, loginviewmodel.Password, false, false))
                .ReturnsAsync(signInResult);

            var userProfile = new UserProfile(applicationUser.Id)
            {
                IsFirstTimeEditedProfile = isFirstTimeEditedProfile
            };
            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(applicationUser.Id)).Returns(userProfile);

            //Act
            var result = await _controller.Login(loginviewmodel, "/") as RedirectToActionResult;

            //Assert
            if (isFirstTimeEditedProfile)
            {
                result.ActionName.Should().Be(nameof(HomeController.Index));
                result.ControllerName.Should().Be("Home");
            }
            else
            {
                result.ActionName.Should().Be(nameof(ProfileController.FirstTimeEditProfile));
                result.ControllerName.Should().Be("Profile");
            }
        }

        [Fact]
        public async void Login_Returns_View_With_Error_If_Is_Not_User_Found()
        {
            var loginviewmodel = GetLoginViewModel();

            var signInResult = new SignInResult();
            signInResult.Set(nameof(SignInResult.Succeeded), false);

            _signInManagerMock.Setup(
                    x => x.PasswordSignInAsync(loginviewmodel.Email, loginviewmodel.Password, false, false))
                .ReturnsAsync(signInResult);

            //Act
            var result = await _controller.Login(loginviewmodel, "/") as ViewResult;

            //Assert
            result.Model.Should().Be(loginviewmodel);
            result.ViewData.Values.Last().Should().Be("/");
            AssertModelStateContainsError(result, "Invalid login attempt.");

            _userManagerMock.Verify(m => m.IsEmailConfirmedAsync(It.IsAny<ApplicationUser>()), Times.Never);
            QueryProcessorMock.Verify(p => p.FindById<UserProfile>(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void Register_Sets_ReturnUrl_ViewData_And_Returns_View()
        {
            var returnUrl = Guid.NewGuid().ToString();

            //Act
            var result = _controller.Register(returnUrl) as ViewResult;

            //Assert
            Assert.Equal(result.ViewData.Values.Last(), returnUrl);
        }

        [Fact]
        public async void Register_Returns_View_And_Model_When_ModelState_Is_Invalid()
        {
            var registerviewmodel = GetRegisterViewModel();
            _controller.ViewData.ModelState.AddModelError("", "mock error message");

            //Act
            var result = await _controller.Register(registerviewmodel, "/") as ViewResult;

            //Assert
            result.Model.Should().Be(registerviewmodel);
            result.ViewData.Values.Last().Should().Be("/");
            _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _authMessageSenderMock.Verify(m => m.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async void Register_Returns_Errors_When_User_Creation_Fails()
        {
            var registerviewmodel = GetRegisterViewModel();

            var expectedUser = new ApplicationUser
            {
                UserName = registerviewmodel.Email,
                Email = registerviewmodel.Email
            };
            ApplicationUser createdUser = null;
            var identityError = new IdentityError
            {
                Code = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };
            var identityResult = IdentityResult.Failed(identityError);
            identityResult.Set(nameof(IdentityResult.Succeeded), false);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerviewmodel.Password))
                .Callback<ApplicationUser, string>((user, password) => createdUser = user)
                .ReturnsAsync(identityResult);

            //Act
            var result = await _controller.Register(registerviewmodel, "/") as ViewResult;

            //Assert 
            result.Model.Should().Be(registerviewmodel);
            result.ViewData.Values.Last().Should().Be("/");
            expectedUser.Email.Should().Be(createdUser.Email);
            expectedUser.UserName.Should().Be(createdUser.UserName);
            AssertModelStateContainsError(result, identityError.Description);
            _authMessageSenderMock.Verify(x => x.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async void Register_Creates_And_Signs_In_User_And_Redirects_To_ReturnUrl()
        {
            var registerviewmodel = GetRegisterViewModel();
            var expectedUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = registerviewmodel.Email,
                Email = registerviewmodel.Email                
            };

            ApplicationUser createdUser = null;
            var identityResult = new IdentityResult();
            identityResult.Set(nameof(IdentityResult.Succeeded), true);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerviewmodel.Password))
                .Callback<ApplicationUser, string>((user, password) => createdUser = user)
                .ReturnsAsync(identityResult);

            _authMessageSenderMock.Setup(x => x.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>())).Returns(Task.FromResult(expectedUser));

            CreateUserProfileCommand executedCommand = null;

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<CreateUserProfileCommand>()))
                .Callback<CreateUserProfileCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            //Act
            var result = await _controller.Register(registerviewmodel, "/") as ViewResult;

            //Assert 
            result.ViewName.Should().Be("SentEmailConfirmation");
            expectedUser.Email.Should().Be(createdUser.Email);
            expectedUser.UserName.Should().Be(createdUser.UserName);
            executedCommand.Email.Should().Be(expectedUser.Email);

            _authMessageSenderMock.Verify(x => x.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async void ConfirmEmail_Returns_Error_View_When_UserId_Is_Null_Or_Code_Is_Null()
        {
            //Act
            var result = await _controller.ConfirmEmail(null, null) as ViewResult;

            //Assert
            result.ViewName.Should().Be("Error");
            _userManagerMock.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async void ConfirmEmail_Returns_Error_View_If_Not_Found_User_In_Manager()
        {
            _userManagerMock.Setup(x => x.FindByIdAsync(UserId)).Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            var result = await _controller.ConfirmEmail(UserId, Code) as ViewResult;

            //Assert
            result.ViewName.Should().Be("Error");
            _userManagerMock.Verify(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async void ConfirmEmail_Confirms_And_Returns_CorfirmEmail_View()
        {
            var identityResult = new IdentityResult();
            identityResult.Set(nameof(IdentityResult.Succeeded), true);

            _userManagerMock.Setup(x => x.FindByIdAsync(UserId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), Code))
                .ReturnsAsync(identityResult);

            //Act
            var result = await _controller.ConfirmEmail(UserId, Code) as ViewResult;

            //Assert
            result.ViewName.Should().Be("ConfirmEmail");
        }

        [Fact]
        public async void ConfirmEmail_Returns_Error_View_When_Not_Confirms()
        {
            var identityResult = new IdentityResult();
            identityResult.Set(nameof(IdentityResult.Succeeded), false);

            _userManagerMock.Setup(x => x.FindByIdAsync(UserId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), Code))
                .ReturnsAsync(identityResult);

            //Act
            var result = await _controller.ConfirmEmail(UserId, Code) as ViewResult;

            //Assert
            result.ViewName.Should().Be("Error");
        }

        [Fact]
        public async void ForgotPassword_Returns_View_And_Model_When_ModelState_Is_Invalid()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel()
            {
                Email = "test"
            };
            _controller.ViewData.ModelState.AddModelError("", "mock error message");

            //Act
            var result = await _controller.ForgotPassword(forgotPasswordViewModel) as ViewResult;

            //Assert
            result.Model.Should().Be(forgotPasswordViewModel);
        }

        [Fact]
        public async void ForgotPassword_Returns_ForgotPasswordConfirmation_View_If_User_Is_Not_Found()
        {
            var forgotpasswordviewmodel = new ForgotPasswordViewModel()
            {
                Email = "test@test.com"
            };

            var identity = new ApplicationUser();

            _userManagerMock.Setup(x => x.FindByNameAsync(forgotpasswordviewmodel.Email))
                .Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            var result = await _controller.ForgotPassword(forgotpasswordviewmodel) as ViewResult;

            //Assert
            result.ViewName.Should().Be("ForgotPasswordConfirmation");
            _userManagerMock.Verify(x => x.IsEmailConfirmedAsync(identity), Times.Never);
        }

        [Fact]
        public async void ForgotPassword_Sends_Password_Reset_Mail_To_User_And_Results_ForgotPasswordConfirmation_View()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel()
            {
                Email = "test@test.com"
            };
            var identity = new ApplicationUser();

            _userManagerMock.Setup(x => x.FindByNameAsync(forgotPasswordViewModel.Email))
                .ReturnsAsync(identity);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(identity)).ReturnsAsync(true);
            _authMessageSenderMock.Setup(x => x.SendForgotPasswordMessageAsync(identity)).Returns(Task.FromResult(identity));
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Scheme = "http";


            //Act
            var result = await _controller.ForgotPassword(forgotPasswordViewModel) as ViewResult;

            //Assert
            result.ViewName.Should().Be("ForgotPasswordConfirmation");
        }

        private LoginViewModel GetLoginViewModel()
        {
            return new LoginViewModel
            {
                Email = "test@test.com",
                Password = "testLoginViewModelPassword"
            };
        }

        private RegisterViewModel GetRegisterViewModel()
        {
            return new RegisterViewModel
            {
                Email = "test@test.com",
                Password = "testRegisterViewModelPassword",
                ConfirmPassword = "testRegisterViewModelCorfirmPassword"
            };
        }
    }
}
