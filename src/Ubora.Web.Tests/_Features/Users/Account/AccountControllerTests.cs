using System;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Services;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Users.Account;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.Account
{
    public class AccountControllerTests
    {
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<IOptions<IdentityCookieOptions>> _identityCookieOptionsMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<ISmsSender> _smsSenderMock;
        private readonly Mock<ICommandProcessor> _commandProcessorMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _identityCookieOptionsMock = new Mock<IOptions<IdentityCookieOptions>>();
            _identityCookieOptionsMock.Setup(o => o.Value).Returns(new IdentityCookieOptions());
            _emailSenderMock = new Mock<IEmailSender>();
            _smsSenderMock = new Mock<ISmsSender>();
            _commandProcessorMock = new Mock<ICommandProcessor>();
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object,
                _identityCookieOptionsMock.Object, _emailSenderMock.Object, _smsSenderMock.Object,
                _loggerFactoryMock.Object, _commandProcessorMock.Object, _queryProcessorMock.Object);
        }

        [Fact]
        public void ViewProfile_Returns_View_And_ProfileViewModel_When_Should_Have_User()
        {
            var userId = Guid.NewGuid();
            UserProfile userprofile = GetUserProfile(userId);

            var expectedProfileViewModel = new ProfileViewModel()
            {
                Email = userprofile.Email,
                FirstName = userprofile.FirstName,
                LastName = userprofile.LastName,
                University = userprofile.University,
                Degree = userprofile.Degree,
                Field = userprofile.Field,
                Biography = userprofile.Biography,
                Skills = userprofile.Skills,
                Role = userprofile.Role
            };

            _queryProcessorMock.Setup(p => p.FindById<UserProfile>(userId))
                .Returns(userprofile);

            //Act
            var result = _controller.ViewProfile(userId);

            //Act
            result.Should().NotBeNull();
            result.As<ViewResult>().Model.As<ProfileViewModel>().Email.Should().Be(expectedProfileViewModel.Email);
            result.As<ViewResult>().Model.As<ProfileViewModel>().FirstName.Should().Be(expectedProfileViewModel.FirstName);
            result.As<ViewResult>().Model.As<ProfileViewModel>().LastName.Should().Be(expectedProfileViewModel.LastName);
            result.As<ViewResult>().Model.As<ProfileViewModel>().University.Should().Be(expectedProfileViewModel.University);
            result.As<ViewResult>().Model.As<ProfileViewModel>().Degree.Should().Be(expectedProfileViewModel.Degree);
            result.As<ViewResult>().Model.As<ProfileViewModel>().Field.Should().Be(expectedProfileViewModel.Field);
            result.As<ViewResult>().Model.As<ProfileViewModel>().Biography.Should().Be(expectedProfileViewModel.Biography);
            result.As<ViewResult>().Model.As<ProfileViewModel>().Skills.Should().Be(expectedProfileViewModel.Skills);
            result.As<ViewResult>().Model.As<ProfileViewModel>().Role.Should().Be(expectedProfileViewModel.Role);
        }

        [Fact]
        public void ViewProfile_Returns_NotFoundResult_If_Not_Found_User()
        {
            var userId = Guid.NewGuid();

            _queryProcessorMock.Setup(p => p.FindById<UserProfile>(userId));

            //Act
            var result = _controller.ViewProfile(userId);

            //Act
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        private UserProfile GetUserProfile(Guid userId)
        {
            return new UserProfile(userId)
            {
                Email = "expectedEmail",
                FirstName = "expectedFirstName",
                LastName = "expectedLastName",
                University = "expectedUniversity",
                Degree = "expectedDegree",
                Field = "expectedField",
                Biography = "expectedBiography",
                Skills = "expectedSkills",
                Role = "expectedRole",
            };
        }
    }
}
