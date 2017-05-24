using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Users.UserList;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.UserList
{
    public class UserListViewModelTests
    {
        private readonly Mock<IQueryProcessor> _queryProcessorMock;
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly UserListViewModel.Factory _factory;

        public UserListViewModelTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _userManagerMock = new Mock<FakeUserManager>();
            _factory = new UserListViewModel.Factory(_queryProcessorMock.Object, _userManagerMock.Object);
        }

        [Fact]
        public void GetUserListItemViewModels_Returns_UserListItemViewModels()
        {
            var expectedFirstUserId = Guid.NewGuid();
            var expectedSecoundUserId = Guid.NewGuid();
            var userProfiles = GetUserProfiles(expectedFirstUserId, expectedSecoundUserId);
            var expectedUserListItemViewModels = GetUserListItemViewModels(expectedFirstUserId, expectedSecoundUserId);

            _queryProcessorMock.Setup(p => p.Find<UserProfile>(null)).Returns(userProfiles);

            _userManagerMock.Setup(m => m.FindByIdAsync(expectedFirstUserId.ToString()))
                .Returns(Task.FromResult(new ApplicationUser {Email = "expectedEmail0"}));

            _userManagerMock.Setup(m => m.FindByIdAsync(expectedSecoundUserId.ToString()))
                .Returns(Task.FromResult(new ApplicationUser {Email = "expectedEmail1"}));

            //Act
            var result = _factory.GetUserListItemViewModels();

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Select(r => r.UserId).Should().Equal(expectedUserListItemViewModels.Select(m => m.UserId));
            result.Select(r => r.FullName).Should().Equal(expectedUserListItemViewModels.Select(m => m.FullName));
            result.Select(r => r.Email).Should().Equal(expectedUserListItemViewModels.Select(m => m.Email));
        }

        [Fact]
        public void GetUserListItemViewModels_Returns_UserListItemViewModels_When_UserProfiles_Is_Empty()
        {
            _queryProcessorMock.Setup(p => p.Find<UserProfile>(null)).Returns(new List<UserProfile>());

            //Act
            var result = _factory.GetUserListItemViewModels();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetUserListItemViewModels_Returns_UserListItemViewModels_And_Email_Not_Found_When_UserId_Is_Wrong()
        {
            var expectedFirstUserId = Guid.NewGuid();
            var expectedSecoundUserId = Guid.NewGuid();
            var userProfiles = GetUserProfiles(expectedFirstUserId, expectedSecoundUserId);
            var expectedUserListItemViewModels = GetUserListItemViewModels(expectedFirstUserId, expectedSecoundUserId);

            _queryProcessorMock.Setup(p => p.Find<UserProfile>(null)).Returns(userProfiles);

            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()));

            _userManagerMock.Setup(m => m.FindByIdAsync(expectedSecoundUserId.ToString()))
                .Returns(Task.FromResult(new ApplicationUser { Email = "expectedEmail1" }));

            //Act
            var result = _factory.GetUserListItemViewModels();

            //Assert
            result.Should().NotBeNullOrEmpty();
        }

        private IEnumerable<UserListViewModel.UserListItemViewModel> GetUserListItemViewModels(params Guid[] userIds)
        {
            var userListItemViewModel = new List<UserListViewModel.UserListItemViewModel>();

            for (var i = 0; i < userIds.Length; i++)
            {
                userListItemViewModel.Add(new UserListViewModel.UserListItemViewModel
                {
                    UserId = userIds[i],
                    FullName = "expectedFirstName" + i + " expectedLastName" + i,
                    Email = "expectedEmail" + i
                });
            }

            return userListItemViewModel.AsEnumerable();
        }

        private IEnumerable<UserProfile> GetUserProfiles(params Guid[] userIds)
        {
            var userProfiles = new List<UserProfile>();

            for (var i = 0; i < userIds.Length; i++)
            {
                userProfiles.Add(new UserProfile(userIds[i])
                {
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
