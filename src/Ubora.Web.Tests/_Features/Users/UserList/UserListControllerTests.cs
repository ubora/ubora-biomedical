using System;
using System.Collections.Generic;
using System.Linq;
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
            var expectedFirstUserId = Guid.NewGuid();
            var expectedSecoundUserId = Guid.NewGuid();

            var userProfiles = GetUserProfiles(expectedFirstUserId, expectedSecoundUserId);
            var expectedUserListItemViewModels = GetUserListItemViewModels(expectedFirstUserId, expectedSecoundUserId);

            _processorMock.Setup(p => p.Find<UserProfile>(null)).Returns(userProfiles);
            _mapperMock.Setup(m => m.Map(userProfiles, It.IsAny<List<UserListItemViewModel>>())).Returns(expectedUserListItemViewModels);

            //Act
            var result = (ViewResult)_controller.Index();

            //Assert
            result.Model.Should().Be(expectedUserListItemViewModels);
        }

        [Fact]
        public void Index_Returns_Empty_Users_List_If_No_Users_Found()
        {
            var expectedEmptyUserList = new List<UserListItemViewModel>();
            var userProfiles = GetUserProfiles();

            _processorMock.Setup(p => p.Find<UserProfile>(null)).Returns(userProfiles);
            _mapperMock.Setup(m => m.Map(userProfiles, It.IsAny<List<UserListItemViewModel>>())).Returns(expectedEmptyUserList);

            //Act
            var result = _controller.Index();

            //Assert
            result.As<ViewResult>().Model.Should().Be(expectedEmptyUserList);
        }

        private List<UserListItemViewModel> GetUserListItemViewModels(params Guid[] userIds)
        {
            var userListItemViewModel = new List<UserListItemViewModel>();

            for (var i = 0; i < userIds.Length; i++)
            {
                userListItemViewModel.Add(new UserListItemViewModel
                {
                    UserId = userIds[i],
                    FullName = "expectedFirstName" + i + " expectedLastName" + i,
                    Email = "expectedEmail" + i
                });
            }

            return userListItemViewModel;
        }

        private IEnumerable<UserProfile> GetUserProfiles(params Guid[] userIds)
        {
            var userProfiles = new List<UserProfile>();

            for (var i = 0; i < userIds.Length; i++)
            {
                userProfiles.Add(new UserProfile(userIds[i])
                {
                    Email = "expectedEmail" + i,
                    FirstName = "expectedFirstName" + i,
                    LastName = "expectedLastName" + i,
                    University = "expectedUniversity" + i,
                    Degree = "expectedDegree" + i,
                    Biography = "expectedBiography" + i,
                    Skills = "expectedSkills" + i,
                    Role = "expectedRole" + i
                });
            }

            return userProfiles.AsEnumerable();
        }
    }
}
