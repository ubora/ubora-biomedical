using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;
using Ubora.Web._Features.Home;
using Ubora.Web._Features.Users.Account;
using Ubora.Web._Features.Users.Profile;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web.Tests._Features.Users.Account
{
    public class AccountControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<IApplicationUserManager> _userManagerMock;
        private readonly Mock<IApplicationSignInManager> _signInManagerMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<ILogger<AccountController>> _loggerMock;
        private readonly Mock<IEmailConfirmationMessageSender> _confirmationMessageSenderMock;
        private readonly Mock<IPasswordRecoveryMessageSender> _passwordRecoveryMessageSenderMock;
        private readonly Mock<ICommandProcessor> _commandProcessor;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _userManagerMock = new Mock<IApplicationUserManager>(MockBehavior.Strict);
            _signInManagerMock = new Mock<IApplicationSignInManager>(MockBehavior.Strict);
            _emailSenderMock = new Mock<IEmailSender>();
            _loggerMock = new Mock<ILogger<AccountController>>();
            _confirmationMessageSenderMock = new Mock<IEmailConfirmationMessageSender>(MockBehavior.Strict);
            _passwordRecoveryMessageSenderMock = new Mock<IPasswordRecoveryMessageSender>(MockBehavior.Strict);
            _commandProcessor = new Mock<ICommandProcessor>();
            var urlHelperMock = new Mock<IUrlHelper>();
            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object,
                 _emailSenderMock.Object,
                _loggerMock.Object, _commandProcessor.Object, _confirmationMessageSenderMock.Object, _passwordRecoveryMessageSenderMock.Object)
            {
                Url = urlHelperMock.Object
            };
            SetUpForTest(_controller);
        }

        [Fact]
        public async Task Login_Returns_Login_View_And_ViewModel_When_ModelState_Is_Invalid()
        {
            var loginViewModel = new LoginViewModel
            {
                Email = "test@test.com",
                Password = "testLoginViewModelPassword"
            };

            const string errorMessage = "You must enter a valid email address";
            _controller.ViewData.ModelState.AddModelError("", errorMessage);
            var returnUrl = "/UserList/Index";

            _userManagerMock.Setup(m => m.FindByNameAsync(loginViewModel.Email)).ReturnsAsync((ApplicationUser)null);

            //Act
            var result = (ViewResult)await _controller.Login(loginViewModel, returnUrl);

            //Assert
            result.Model.Should().Be(loginViewModel);
            result.ViewData.Values.Last().Should().Be(returnUrl);
            AssertModelStateContainsError(result, errorMessage);

            _userManagerMock.Verify(m => m.FindByNameAsync(It.IsAny<string>()), Times.Never);
            _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
            QueryProcessorMock.Verify(p => p.FindById<UserProfile>(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Login_Logs_In_And_Redirects_To_ReturnUrl_When_Valid_ModelState()
        {
            var loginViewModel = GetLoginViewModel();

            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid()
            };
            _userManagerMock.Setup(x => x.FindByNameAsync(loginViewModel.Email)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(applicationUser)).ReturnsAsync(true);
            _signInManagerMock.Setup(
                    x => x.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false))
                .ReturnsAsync(SignInResult.Success);

            var returnUrl = "/UserList/Index";

            //Act
            var result = (RedirectToActionResult)await _controller.Login(loginViewModel, returnUrl);

            //Assert
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");

            _loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Contains($"{loginViewModel.Email} is the email of the user who logged in.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task Login_Logs_In_And_Redirects_To_ReturnUrl_When_Profile_Is_First_Time_Edited_And_Valid_ModelState()
        {
            var loginViewModel = GetLoginViewModel();

            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid()
            };
            _userManagerMock.Setup(x => x.FindByNameAsync(loginViewModel.Email)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(applicationUser)).ReturnsAsync(true);
            _signInManagerMock.Setup(
                    x => x.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false))
                .ReturnsAsync(SignInResult.Success);

            var returnUrl = "/UserList/Index";

            //Act
            var result = (RedirectToActionResult)await _controller.Login(loginViewModel, returnUrl);

            //Assert
            result.ActionName.Should().Be(nameof(HomeController.Index));
            result.ControllerName.Should().Be("Home");

            _loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Contains($"{loginViewModel.Email} is the email of the user who logged in.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task Login_Returns_Login_View_With_Error_When_Login_Fails()
        {
            var loginViewModel = GetLoginViewModel();

            _signInManagerMock.Setup(
                    x => x.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false))
                .ReturnsAsync(SignInResult.Failed);

            var returnUrl = "/UserList/Index";

            //Act
            var result = (ViewResult)await _controller.Login(loginViewModel, returnUrl);

            //Assert
            result.Model.Should().Be(loginViewModel);
            result.ViewData.Values.Last().Should().Be(returnUrl);
            AssertModelStateContainsError(result, "Invalid login attempt.");

            _userManagerMock.Verify(m => m.IsEmailConfirmedAsync(It.IsAny<ApplicationUser>()), Times.Never);
            QueryProcessorMock.Verify(p => p.FindById<UserProfile>(It.IsAny<Guid>()), Times.Never);
            _loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Contains($"{loginViewModel.Email} is the email of the user who logged in.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Never
            );
        }

        [Fact]
        public void Register_Returns_Register_View()
        {
            var returnUrl = Guid.NewGuid().ToString();

            //Act
            var result = _controller.Register(returnUrl) as ViewResult;

            //Assert
            Assert.Equal(result.ViewData.Values.Last(), returnUrl);
        }

        [Fact]
        public async Task Register_Returns_Register_View_And_Model_When_ModelState_Is_Invalid()
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
            _confirmationMessageSenderMock.Verify(m => m.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task Register_Returns_Errors_When_User_Creation_Fails()
        {
            var registerViewModel = GetRegisterViewModel();

            var expectedUser = new ApplicationUser
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };

            var identityError = new IdentityError
            {
                Code = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };
            var identityResult = IdentityResult.Failed(identityError);

            ApplicationUser createdUser = null;
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
            _confirmationMessageSenderMock.Verify(x => x.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task Register_Creates_And_Signs_In_And_Redirects_To_SentEmaiConfirmation()
        {
            var registerViewModel = GetRegisterViewModel();
            var expectedUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };

            ApplicationUser createdUser = null;
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerViewModel.Password))
                .Callback<ApplicationUser, string>((user, password) => createdUser = user)
                .ReturnsAsync(IdentityResult.Success);

            _confirmationMessageSenderMock.Setup(x => x.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>())).Returns(Task.FromResult(expectedUser));

            CreateUserProfileCommand executedCommand = null;

            _commandProcessor
                .Setup(p => p.Execute(It.IsAny<CreateUserProfileCommand>()))
                .Callback<CreateUserProfileCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            _signInManagerMock.Setup(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, null))
                .Returns(Task.FromResult(new ApplicationUser()));

            var returnUrl = "/UserList/Index";

            //Act
            var result = (RedirectToActionResult)await _controller.Register(registerViewModel, returnUrl);

            //Assert 
            result.ActionName.Should().Be(nameof(ProfileController.FirstTimeEditProfile));
            result.ControllerName.Should().Be("Profile");
            result.RouteValues.Values.Single().Should().Be(returnUrl);
            expectedUser.Email.Should().Be(createdUser.Email);
            expectedUser.UserName.Should().Be(createdUser.UserName);
            executedCommand.Email.Should().Be(expectedUser.Email);

            _confirmationMessageSenderMock.Verify(x => x.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Once);
            _loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Contains($"{registerViewModel.Email} is the email of the user who created a new account with password.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                )
            );
            _signInManagerMock.Verify(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, null), Times.Once);
        }

        [Fact]
        public async Task ConfirmEmail_Throws_InvalidOperationException_When_User_Is_Not_Found()
        {
            var userId = Guid.NewGuid().ToString();
            var code = "testCode";

            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            Func<Task> act = async () => await _controller.ConfirmEmail(userId, code);

            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(act);
            _userManagerMock.Verify(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never);
            _signInManagerMock.Verify(m => m.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never());
            _signInManagerMock.Verify(m => m.SignOutAsync(), Times.Never);
        }

        [Fact]
        public async Task ConfirmEmail_Redirects_To_Home_Index_With_ErrorNotice_When_Confirmation_Is_Not_Successful()
        {
            var userId = "userId";
            var code = "testCode";

            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), code))
                .ReturnsAsync(IdentityResult.Failed());

            //Act
            var result = (RedirectToActionResult)await _controller.ConfirmEmail(userId, code);

            //Assert
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");

            var errorNotice = _controller.Notices.Dequeue();
            errorNotice.Text.Should().Be("Confirmation code is wrong or expired!");
            errorNotice.Type.Should().Be(NoticeType.Error);

            _userManagerMock.Verify(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()), Times.Never);
            _signInManagerMock.Verify(m => m.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never());
            _signInManagerMock.Verify(m => m.SignOutAsync(), Times.Never);
        }

        [Fact]
        public async Task ConfirmEmail_Refreshes_SignIn_And_Redirects_To_Home_Index_With_SuccessNotice_When_User_Is_Authenticated_With_Successful_Email_Confirmation()
        {
            var userId = UserId.ToString();
            var code = "testCode";

            var applicationUser = new ApplicationUser();
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(applicationUser, code))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.GetUserId(User)).Returns(userId);
            _signInManagerMock.Setup(m => m.RefreshSignInAsync(applicationUser))
                .Returns(Task.FromResult(applicationUser));

            //Act
            var result = (RedirectToActionResult)await _controller.ConfirmEmail(userId, code);

            //Assert
            result.ControllerName.Should().Be("Home");
            result.ActionName.Should().Be("Index");

            _signInManagerMock.Verify(m => m.RefreshSignInAsync(applicationUser), Times.Once);
            _signInManagerMock.Verify(m => m.SignOutAsync(), Times.Never);

            var successNotice = _controller.Notices.Dequeue();
            successNotice.Text.Should().Be("Your email has been confirmed successfully!");
            successNotice.Type.Should().Be(NoticeType.Success);
        }

        [Fact]
        public async Task ConfirmEmail_Signs_Out_And_Redirects_To_Home_Index_With_SuccessNotice_When_User_Is_Not_Authenticated()
        {
            var userId = Guid.NewGuid().ToString();
            var code = "testCode";

            var applicationUser = new ApplicationUser();
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(applicationUser, code))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.GetUserId(User)).Returns(userId);
            _signInManagerMock.Setup(m => m.SignOutAsync())
                .Returns(Task.FromResult(applicationUser));

            //Act
            var result = (RedirectToActionResult)await _controller.ConfirmEmail(userId, code);

            //Assert
            result.ControllerName.Should().Be("Home");
            result.ActionName.Should().Be("Index");

            var successNotice = _controller.Notices.Dequeue();
            successNotice.Text.Should().Be("Your email has been confirmed successfully!");
            successNotice.Type.Should().Be(NoticeType.Success);

            _signInManagerMock.Verify(m => m.SignOutAsync(), Times.Once);
            _signInManagerMock.Verify(m => m.RefreshSignInAsync(applicationUser), Times.Never);
        }

        [Fact]
        public async Task ConfirmEmail_Redirects_To_Home_Index_When_Email_Is_Already_Confirmed()
        {
            var userId = "userId";
            var applicationUser = new ApplicationUser()
            {
                EmailConfirmed = true
            };
            _userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(applicationUser);

            //Act
            var result = (RedirectToActionResult)await _controller.ConfirmEmail(userId, "code");

            //Assert
            result.ControllerName.Should().Be("Home");
            result.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task ForgotPassword_Returns_View_And_Model_When_ModelState_Is_Invalid()
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
        public async Task ForgotPassword_Returns_ForgotPasswordConfirmation_View_If_User_Is_Not_Found()
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
        public async Task ForgotPassword_Sends_Password_Reset_Mail_To_User_And_Results_ForgotPasswordConfirmation_View()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "test@test.com"
            };

            var identity = new ApplicationUser();

            _userManagerMock.Setup(x => x.FindByNameAsync(forgotPasswordViewModel.Email))
                .ReturnsAsync(identity);
            _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(identity)).ReturnsAsync(true);
            _passwordRecoveryMessageSenderMock.Setup(x => x.SendForgotPasswordMessage(identity)).Returns(Task.FromResult(identity));
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Scheme = "http";


            //Act
            var result = (ViewResult)await _controller.ForgotPassword(forgotPasswordViewModel);

            //Assert
            result.ViewName.Should().Be("ForgotPasswordConfirmation");
        }

        [Fact]
        public async Task ResendEmailConfirmation_Resends_Email_Confirmation_Message_And_Returns_View()
        {
            var applicationUser = new ApplicationUser();
            _userManagerMock.Setup(m => m.GetUserAsync(User)).ReturnsAsync(applicationUser);
            _confirmationMessageSenderMock.Setup(s => s.SendEmailConfirmationMessage(applicationUser))
                .Returns(Task.FromResult(applicationUser));

            //Act
            var result = (ViewResult)await _controller.ResendEmailConfirmation();

            //Assert
            _confirmationMessageSenderMock.Verify(s => s.SendEmailConfirmationMessage(applicationUser), Times.Once);
        }

        [Fact]
        public async Task ResendEmailConfirmation_Redirects_To_Home_Index_When_User_Is_Not_Found()
        {
            _userManagerMock.Setup(m => m.GetUserAsync(User)).ReturnsAsync((ApplicationUser)null);

            //Act
            var result = (RedirectToActionResult)await _controller.ResendEmailConfirmation();

            //Assert
            _confirmationMessageSenderMock.Verify(s => s.SendEmailConfirmationMessage(It.IsAny<ApplicationUser>()), Times.Never);
        }

        private LoginViewModel GetLoginViewModel()
        {
            return new LoginViewModel
            {
                Email = "test@test.com",
                Password = "expectedPassword"
            };
        }

        private RegisterViewModel GetRegisterViewModel()
        {
            return new RegisterViewModel
            {
                Email = "test@test.com",
                Password = "expectedpassword",
                ConfirmPassword = "expectedConfirmPassWord"
            };
        }
    }
}
