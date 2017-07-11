using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TwentyTwenty.Storage;
using Ubora.Domain.Users;
using Ubora.Web._Features.Users.UserList;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.UserList
{
    public class UserListControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<IStorageProvider> _storageProviderMock;
        private readonly UserListController _controller;

        public UserListControllerTests()
        {
            _storageProviderMock = new Mock<IStorageProvider>();
            _controller = new UserListController(_storageProviderMock.Object);
            SetUpForTest(_controller);
        }

        [Theory]
        [InlineData("test.jpg")]
        [InlineData(null)]
        public void Index_Returns_Users(string blobname)
        {
            var userProfile = new UserProfile(Guid.NewGuid());

            if (blobname != null)
            {
                userProfile.ProfilePictureBlobName = "test.jpg";
            }

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var url = $"/app/wwwroot/images/storages/users/{userProfile.UserId}/profile-pictures/test.jpg";
            var expectedUrl = $"/app/wwwroot/images/storages/users/{userProfile.UserId}/profile-pictures/test.jpg";

            if (blobname != null)
            {
                _storageProviderMock.Setup(p => p.GetBlobUrl(It.IsAny<string>(), It.IsAny<string>())).Returns(url);
            }

            QueryProcessorMock.Setup(p => p.Find<UserProfile>(null)).Returns(userProfiles);

            //Act
            var result = (ViewResult)_controller.Index();

            //Assert
            result.Model.As<IEnumerable<UserListItemViewModel>>().Last().UserId.Should().Be(userProfile.UserId);
            result.Model.As<IEnumerable<UserListItemViewModel>>().Last().Email.Should().Be(userProfile.Email);
            result.Model.As<IEnumerable<UserListItemViewModel>>().Last().FullName.Should().Be(userProfile.FullName);
            result.Model.As<IEnumerable<UserListItemViewModel>>()
                .Last()
                .ProfilePictureLink.Should()
                .Be(blobname != null ? expectedUrl : "/images/profileimagedefault.png");
        }
    }
}
