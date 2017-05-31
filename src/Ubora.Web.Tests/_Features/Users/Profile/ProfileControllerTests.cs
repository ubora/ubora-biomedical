using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users;
using Ubora.Web._Features.Users.Profile;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.Profile
{
    public class ProfileControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICommandQueryProcessor> _commandQueryProcessorMock;
        private readonly ProfileController _controller;

        public ProfileControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _commandQueryProcessorMock = new Mock<ICommandQueryProcessor>();
            _controller = new ProfileController(_commandQueryProcessorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void ViewProfile_Returns_View_And_ProfileViewModel_When_Should_Have_User()
        {
            var userId = Guid.NewGuid();
            var userprofile = new UserProfile(userId);
            var expectedProfileViewModel = new ProfileViewModel();

            _commandQueryProcessorMock.Setup(p => p.FindById<UserProfile>(userId))
                .Returns(userprofile);

            _mapperMock.Setup(m => m.Map(It.IsAny<UserProfile>(), It.IsAny<ProfileViewModel>()))
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
