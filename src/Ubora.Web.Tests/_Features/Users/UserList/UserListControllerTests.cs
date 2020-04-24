using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Users;
using Ubora.Web._Features.Users.UserList.Models;
using Xunit;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Users.Specifications;
using Ubora.Web._Features.Users.UserList;
using Ubora.Web._Features._Shared.Paging;
using Ubora.Domain.Users.Queries;

namespace Ubora.Web.Tests._Features.Users.UserList
{
    public partial class UserListControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<ImageStorageProvider> _imageStorageProviderMock;
        private readonly UserListController _controller;

        public UserListControllerTests()
        {
            _imageStorageProviderMock = new Mock<ImageStorageProvider>();
            _controller = new UserListController(_imageStorageProviderMock.Object);
            SetUpForTest(_controller);
        }

        [Theory]
        [InlineData("test.jpg")]
        [InlineData(null)]
        public void Search_Returns_Users(string blobname)
        {
            var userProfile = new UserProfile(Guid.NewGuid());

            if (blobname != null)
            {
                userProfile.ProfilePictureBlobLocation = new BlobLocation("test", blobname);
            }

            var userProfiles = new PagedListStub<UserProfile>
            {
                userProfile
            };

            var url = $"/app/wwwroot/images/storages/users/{userProfile.UserId}/profile-pictures/test.jpg";
            var expectedUrl = $"/app/wwwroot/images/storages/users/{userProfile.UserId}/profile-pictures/test.jpg";

            if (blobname != null)
            {
                _imageStorageProviderMock.Setup(p => p.GetUrl(It.IsAny<BlobLocation>()))
                    .Returns(url);
            }
            //var sortSpecifications = new List<ISortSpecification<UserProfile>> { new SortByFirstNameSpecification(SortOrder.Ascending) };
            QueryProcessorMock
                .Setup(p => p.ExecuteQuery(It.Is<SearchUserProfilesQuery>(q => q.SortSpecification != null // new SortByMultipleUserProfileSortSpecification(sortSpecifications) would be more accurate
                                                                                && q.WhereSpecification == new MatchAll<UserProfile>()
                                                                                && q.SearchFullName == null
                                                                                && q.Paging.PageNumber == 2
                                                                                && q.Paging.PageSize == 40)))
                .Returns(userProfiles);
            //Act
            var result = (ViewResult)_controller.Search(new SearchModel(), 2);

            //Assert
            result.Model.As<IndexViewModel>().Ordering.Should().Be(OrderingMethod.Firstname);
            result.Model.As<IndexViewModel>().Tab.Should().Be(TabType.AllMembers);
            result.Model.As<IndexViewModel>().Pager.ShouldBeEquivalentTo(Pager.From(userProfiles));
            result.Model.As<IndexViewModel>().UserListItems.Count().Should().Be(1);
            result.Model.As<IndexViewModel>().UserListItems.Last().UserId.Should().Be(userProfile.UserId);
            result.Model.As<IndexViewModel>().UserListItems.Last().Email.Should().Be(userProfile.Email);
            result.Model.As<IndexViewModel>().UserListItems.Last().FullName.Should().Be(userProfile.FullName);
            result.Model.As<IndexViewModel>().UserListItems.Last()
                .ProfilePictureLink.Should()
                .Be(blobname != null ? expectedUrl : "/images/profileimagedefault.svg");
        }

        [Fact]
        public void SearchUsers_Returns_Users_Json()
        {
            var userProfile1 = new UserProfile(Guid.NewGuid());
            userProfile1.Set(x => x.Email, "Email1");
            userProfile1.Set(x => x.FirstName, "FirstName1");
            userProfile1.Set(x => x.LastName, "LastName1");

            var userProfile2 = new UserProfile(Guid.NewGuid());
            userProfile2.Set(x => x.Email, "Email2");
            userProfile2.Set(x => x.FirstName, "FirstName2");
            userProfile2.Set(x => x.LastName, "LastName2");

            var searchResults = new PagedListStub<UserProfile>
            {
                userProfile1,
                userProfile2
            };

            var searchPhrase = "searchPhrase";
            var specification = new UserFullNameContainsPhraseSpec(searchPhrase)
                || new UserEmailContainsPhraseSpec(searchPhrase);
            QueryProcessorMock.Setup(p => p.Find(specification))
                .Returns(searchResults);

            // Act
            var result = _controller.SearchUsers(searchPhrase);

            // Assert
            var usersDictionary = (Dictionary<string, string>)result.Value;
            usersDictionary.Count.Should().Be(2);

            usersDictionary.TryGetValue("Email1", out string user1FullName);
            user1FullName.Should().Be("FirstName1 LastName1");

            usersDictionary.TryGetValue("Email2", out string user2FullName);
            user2FullName.Should().Be("FirstName2 LastName2");
        }
    }
}
