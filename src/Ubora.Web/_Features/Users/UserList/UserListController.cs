using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Domain.Users.Specifications;
using Ubora.Web._Features._Shared.Paging;
using System.Collections.Generic;
using Ubora.Domain.Users.SortSpecifications;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users.Queries;
using Marten.Pagination;

namespace Ubora.Web._Features.Users.UserList
{
    public class UserListController : UboraController
    {
        private readonly ImageStorageProvider _imageStorageProvider;

        public UserListController(ImageStorageProvider imageStorageProvider)
        {
            _imageStorageProvider = imageStorageProvider;
        }

        public IActionResult Index(SearchModel searchModel, int page = 1)
        {
            var sortSpecifications = new List<ISortSpecification<UserProfile>>();
            switch (searchModel.Ordering)
            {
                case OrderingMethod.Firstname:
                    sortSpecifications.Add(new SortByFirstNameSpecification(SortOrder.Ascending));
                    break;
                case OrderingMethod.Lastname:
                    sortSpecifications.Add(new SortByLastNameSpecification(SortOrder.Ascending));
                    break;
            }

            IPagedList<UserProfile> userProfiles;
            if (searchModel.Tab == TabType.AllMemeber)
            {
                userProfiles = QueryProcessor.Find(new MatchAll<UserProfile>(), new SortByMultipleUserProfileSortSpecification(sortSpecifications), 4, page);
            }
            else
            {
                userProfiles = QueryProcessor.ExecuteQuery(new SortByMultipleUboraMentorProfilesQuery(sortSpecifications, 4, page));
            }

            var userListItemViewModel = userProfiles.Select(userProfile => new UserListItemViewModel
            {
                UserId = userProfile.UserId,
                Email = userProfile.Email,
                FullName = userProfile.FullName,
                Role = userProfile.Role,
                ProfilePictureLink = _imageStorageProvider.GetDefaultOrBlobUrl(userProfile)
            });

            return View(new IndexViewModel
            {
                Ordering = searchModel.Ordering,
                Tab = searchModel.Tab,
                Pager = Pager.From(userProfiles),
                UserListItems = userListItemViewModel
            });
        }

        [HttpGet]
        public JsonResult SearchUsers(string searchPhrase)
        {
            var searchResult = QueryProcessor.Find(new UserFullNameContainsPhraseSpec(searchPhrase)
                    || new UserEmailContainsPhraseSpec(searchPhrase));

            var peopleDictionary = searchResult.ToDictionary(user => user.Email, user => user.FullName);

            return Json(peopleDictionary);
        }

        public class IndexViewModel
        {
            public TabType Tab { get; set; }
            public OrderingMethod Ordering { get; set; }
            public Pager Pager { get; set; }
            public IEnumerable<UserListItemViewModel> UserListItems { get; set; }
        }

        public class SearchModel
        {
            public TabType Tab { get; set; }
            public OrderingMethod Ordering { get; set; }
        }

        public enum TabType
        {
            AllMemeber = 0,
            Mentors = 1
        }

        public enum OrderingMethod
        {
            Firstname = 0,
            Lastname = 1
        }
    }
}
