using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
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
        public async void Login_Returns_Login_View_And_ViewModel_When_ModelState_Is_Invalid()
        {
            var loginViewModel = new LoginViewModel
            {
                Email = "test@test.com",
                Password = "testLoginViewModelPassword"
            };

            const string errorMessage = "You must enter a valid email address";
            _controller.ViewData.ModelState.AddModelError("", errorMessage);
            var returnUrl = "/UserList/Index";

            //Act
            var result = (ViewResult)await _controller.Login(loginViewModel, returnUrl);
            //Assert
            result.Model.Should().Be(loginViewModel);
            result.ViewData.Values.Last().Should().Be(returnUrl);
            AssertModelStateContainsError(result, errorMessage);

            _userManagerMock.Verify(m => m.FindByNameAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(m => m.IsEmailConfirmedAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
            QueryProcessorMock.Verify(p => p.FindById<UserProfile>(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async void Login_Returns_View_With_Error_If_Is_Not_Email_Confirmed()
        {
            var loginViewModel = GetLoginViewModel();

            var applicationUser = new ApplicationUser();
            _userManagerMock.Setup(x => x.FindByNameAsync(loginViewModel.Email)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(applicationUser)).ReturnsAsync(false);

            var returnUrl = "/UserList/Index";

            //Act
            var result = (ViewResult)await _controller.Login(loginViewModel, returnUrl);

            //Assert  
            result.Model.Should().Be(loginViewModel);
            result.ViewData.Values.Last().Should().Be(returnUrl);
            AssertModelStateContainsError(result, "You must have a confirmed email to log in.");

            _signInManagerMock.Verify(
                m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Never);
            QueryProcessorMock.Verify(p => p.FindById<UserProfile>(It.IsAny<Guid>()), Times.Never);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void Login_Signs_In_UserManager_And_Redirects_To_ReturnUrl_When_Are_Success(bool isFirstTimeEditedProfile)
        {
            var loginViewModel = GetLoginViewModel();

            var signInResult = new SignInResult();
            signInResult.Set(nameof(SignInResult.Succeeded), true);

            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid()
            };
            _userManagerMock.Setup(x => x.FindByNameAsync(loginViewModel.Email)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(applicationUser)).ReturnsAsync(true);
            _signInManagerMock.Setup(
                    x => x.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false))
                .ReturnsAsync(signInResult);

            var userProfile = new UserProfile(applicationUser.Id)
            {
                IsFirstTimeEditedProfile = isFirstTimeEditedProfile
            };
            QueryProcessorMock.Setup(p => p.FindById<UserProfile>(applicationUser.Id)).Returns(userProfile);

            //Act
            var result = (RedirectToActionResult)await _controller.Login(loginViewModel, "/");

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
            _loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Contains($"{loginViewModel.Email} is the email of the user who logged in.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                )
            );
        }

        [Fact]
        public async void Login_Returns_View_With_Error_If_Is_Not_User_Found()
        {
            var loginViewModel = GetLoginViewModel();

            var signInResult = new SignInResult();
            signInResult.Set(nameof(SignInResult.Succeeded), false);

            _signInManagerMock.Setup(
                    x => x.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false))
                .ReturnsAsync(signInResult);

            var returnUrl = "/UserList/Index";

            //Act
            var result = (ViewResult)await _controller.Login(loginViewModel, returnUrl);

            //Assert
            result.Model.Should().Be(loginViewModel);
            result.ViewData.Values.Last().Should().Be(returnUrl);
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
            var registerViewModel = GetRegisterViewModel();
            _controller.ViewData.ModelState.AddModelError("", "You must enter a valid email address");

            var returnUrl = "/UserList/Index";

            //Act
            var result = (ViewResult)await _controller.Register(registerViewModel, returnUrl);

            //Assert
            result.Model.Should().Be(registerViewModel);
            result.ViewData.Values.Last().Should().Be(returnUrl);
            _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _authMessageSenderMock.Verify(m => m.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async void Register_Returns_Errors_When_User_Creation_Fails()
        {
            var registerViewModel = GetRegisterViewModel();

            var expectedUser = new ApplicationUser
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };
            ApplicationUser createdUser = null;
            var identityError = new IdentityError
            {
                Code = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };
            var identityResult = IdentityResult.Failed(identityError);
            identityResult.Set(nameof(IdentityResult.Succeeded), false);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerViewModel.Password))
                .Callback<ApplicationUser, string>((user, password) => createdUser = user)
                .ReturnsAsync(identityResult);

            var returnUrl = "/UserList/Index";

            //Act
            var result = (ViewResult)await _controller.Register(registerViewModel, returnUrl);

            //Assert 
            result.Model.Should().Be(registerViewModel);
            result.ViewData.Values.Last().Should().Be(returnUrl);
            expectedUser.Email.Should().Be(createdUser.Email);
            expectedUser.UserName.Should().Be(createdUser.UserName);
            AssertModelStateContainsError(result, identityError.Description);
            _authMessageSenderMock.Verify(x => x.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async void Register_Creates_And_Signs_In_User_And_Redirects_To_ReturnUrl()
        {
            var registerViewModel = GetRegisterViewModel();
            var expectedUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };

            ApplicationUser createdUser = null;
            var identityResult = new IdentityResult();
            identityResult.Set(nameof(IdentityResult.Succeeded), true);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerViewModel.Password))
                .Callback<ApplicationUser, string>((user, password) => createdUser = user)
                .ReturnsAsync(identityResult);

            _authMessageSenderMock.Setup(x => x.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>())).Returns(Task.FromResult(expectedUser));

            CreateUserProfileCommand executedCommand = null;

            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<CreateUserProfileCommand>()))
                .Callback<CreateUserProfileCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            var returnUrl = "/UserList/Index";

            //Act
            var result = (ViewResult)await _controller.Register(registerViewModel, returnUrl);

            //Assert 
            result.ViewName.Should().Be("SentEmailConfirmation");
            expectedUser.Email.Should().Be(createdUser.Email);
            expectedUser.UserName.Should().Be(createdUser.UserName);
            executedCommand.Email.Should().Be(expectedUser.Email);

            _authMessageSenderMock.Verify(x => x.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Once);
            _loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Contains($"{registerViewModel.Email} is the email of the user who created a new account with password.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                )
            );
        }

        [Fact]
        public async void ConfirmEmail_Returns_Error_View_When_UserId_Is_Null_Or_Code_Is_Null()
        {
            //Act
            var result = (ViewResult)await _controller.ConfirmEmail(null, null);

            //Assert
            result.ViewName.Should().Be("Error");
            _userManagerMock.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async void ConfirmEmail_Returns_Error_View_If_Not_Found_User_In_UserManager()
        {
            const string userId = "testUserId";
            const string code = "testCode";

            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            var result = (ViewResult)await _controller.ConfirmEmail(userId, code);

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
            const string userId = "testUserId";
            const string code = "testCode";

            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), code))
                .ReturnsAsync(identityResult);

            //Act
            var result = (ViewResult)await _controller.ConfirmEmail(userId, code);

            //Assert
            result.ViewName.Should().Be("ConfirmEmail");
        }

        [Fact]
        public async void ConfirmEmail_Returns_Error_View_When_Not_Confirms()
        {
            var identityResult = new IdentityResult();
            identityResult.Set(nameof(IdentityResult.Succeeded), false);
            const string userId = "testUserId";
            const string code = "testCode";

            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), code))
                .ReturnsAsync(identityResult);

            //Act
            var result = (ViewResult)await _controller.ConfirmEmail(userId, code);

            //Assert
            result.ViewName.Should().Be("Error");
        }

        [Fact]
        public async void ForgotPassword_Returns_View_And_Model_When_ModelState_Is_Invalid()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "test"
            };
            _controller.ViewData.ModelState.AddModelError("", "You must enter a valid email address");

            //Act
            var result = (ViewResult)await _controller.ForgotPassword(forgotPasswordViewModel);

            //Assert
            result.Model.Should().Be(forgotPasswordViewModel);
        }

        [Fact]
        public async void ForgotPassword_Returns_ForgotPasswordConfirmation_View_If_User_Is_Not_Found()
        {
            var forgotpasswordviewmodel = new ForgotPasswordViewModel
            {
                Email = "test@test.com"
            };

            var identity = new ApplicationUser();

            _userManagerMock.Setup(x => x.FindByNameAsync(forgotpasswordviewmodel.Email))
                .Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            var result = (ViewResult)await _controller.ForgotPassword(forgotpasswordviewmodel);

            //Assert
            result.ViewName.Should().Be("ForgotPasswordConfirmation");
            _userManagerMock.Verify(x => x.IsEmailConfirmedAsync(identity), Times.Never);
        }

        [Fact]
        public async void ForgotPassword_Sends_Password_Reset_Mail_To_User_And_Results_ForgotPasswordConfirmation_View()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel
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
            var result = (ViewResult)await _controller.ForgotPassword(forgotPasswordViewModel);

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
