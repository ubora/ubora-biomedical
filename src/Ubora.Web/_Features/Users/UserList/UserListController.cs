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

namespace Ubora.Web._Features.Users.UserList
{
    public class UserListController : UboraController
    {
        private readonly ImageStorageProvider _imageStorageProvider;

        public UserListController(ImageStorageProvider imageStorageProvider)
        {
            _imageStorageProvider = imageStorageProvider;
        }

        public IActionResult Index(int page = 1)
        {
            var userProfiles = QueryProcessor.Find(new MatchAll<UserProfile>(), new SortByFullNameAscendingSpecification(), 24, page);

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
            public Pager Pager { get; set; }
            public IEnumerable<UserListItemViewModel> UserListItems { get; set; }
        }
    }
}
