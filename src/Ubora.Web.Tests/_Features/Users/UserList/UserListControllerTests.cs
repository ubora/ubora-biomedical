using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users;
using Ubora.Web._Features.Users.UserList;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.UserList
{
    public class UserListControllerTests
    {
        private readonly Mock<ICommandQueryProcessor> _processorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserListController _controller;

        public UserListControllerTests()
        {
            _processorMock = new Mock<ICommandQueryProcessor>();
            _mapperMock = new Mock<IMapper>();
            _controller = new UserListController(_processorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void Index_Returns_Users()
        {
            var userProfiles = new List<UserProfile>();
            var expectedUserListItemViewModels = new List<UserListItemViewModel>();

            _processorMock.Setup(p => p.Find<UserProfile>(null)).Returns(userProfiles);
            _mapperMock.Setup(m => m.Map(It.IsAny<IEnumerable<UserProfile>>(), It.IsAny<List<UserListItemViewModel>>())).Returns(expectedUserListItemViewModels);

            //Act
            var result = (ViewResult)_controller.Index();

            //Assert
            result.Model.Should().Be(expectedUserListItemViewModels);
        }
    }
}
