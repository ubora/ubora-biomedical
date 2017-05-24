using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Web._Features.Users.UserList;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.UserList
{
    public class UserListControllerTests
    {
        private readonly Mock<UserListViewModel.Factory> _modelfactoryMock;
        private readonly UserListController _controller;

        public UserListControllerTests()
        {
            _modelfactoryMock = new Mock<UserListViewModel.Factory>();
            _controller = new UserListController(_modelfactoryMock.Object);
        }

        [Fact]
        public void Index_Returns_Users()
        {
            var expectedFirstUser =
                new UserListViewModel.UserListItemViewModel {Email = "test1@test.com", FullName = "expectedFullName" };
            var expectedSecoundUser =
                new UserListViewModel.UserListItemViewModel {Email = "test2@test.com", FullName = "expectedFullName" };
            var expectedUserList = new List<UserListViewModel.UserListItemViewModel> {expectedFirstUser, expectedSecoundUser};

            _modelfactoryMock.Setup(f => f.GetUserListItemViewModels())
                .Returns(expectedUserList);

            //Act
            var result = _controller.Index();

            //Assert
            result.As<ViewResult>().Model.Should().Be(expectedUserList);
        }

        [Fact]
        public void Index_Returns_Users_When_List_Is_Empty()
        {
            var expectedEmptyUserList = new List<UserListViewModel.UserListItemViewModel>();

            _modelfactoryMock.Setup(f => f.GetUserListItemViewModels())
                .Returns(expectedEmptyUserList);

            //Act
            var result = _controller.Index();

            //Assert
            result.As<ViewResult>().Model.Should().Be(expectedEmptyUserList);
        }
    }
}
