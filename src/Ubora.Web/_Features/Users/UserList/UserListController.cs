using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Domain.Users.Specifications;

namespace Ubora.Web._Features.Users.UserList
{
    public class UserListController : UboraController
    {
        private readonly ImageStorageProvider _imageStorageProvider;

        public UserListController(ImageStorageProvider imageStorageProvider)
        {
            _imageStorageProvider = imageStorageProvider;
        }

        public IActionResult Index()
        {
            var userProfiles = QueryProcessor.Find<UserProfile>(new MatchAll<UserProfile>())
                .OrderBy(u => u.FullName);

            var viewmodel = userProfiles.Select(userProfile => new UserListItemViewModel
            {
                UserId = userProfile.UserId,
                Email = userProfile.Email,
                FullName = userProfile.FullName,
                ProfilePictureLink = _imageStorageProvider.GetDefaultOrBlobUrl(userProfile)
            });

            return View(viewmodel);
        }

        [HttpGet]
        public JsonResult SearchUsers(string searchPhrase)
        {
            var searchResult = QueryProcessor.Find(new UserFullNameContainsPhraseSpec(searchPhrase)
                    || new UserEmailContainsPhraseSpec(searchPhrase));

            var peopleDictionary = searchResult.ToDictionary(user => user.Email, user => user.FullName);

            return Json(peopleDictionary);
        }
    }
}
