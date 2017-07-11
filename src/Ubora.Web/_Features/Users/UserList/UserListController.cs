using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TwentyTwenty.Storage;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Users.UserList
{
    public class UserListController : UboraController
    {
        private readonly IStorageProvider _storageProvider;

        public UserListController(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public IActionResult Index()
        {
            var userProfiles = QueryProcessor.Find<UserProfile>();

            var viewmodel = userProfiles.Select(userProfile => new UserListItemViewModel
            {
                UserId = userProfile.UserId,
                Email = userProfile.Email,
                FullName = userProfile.FullName,
                ProfilePictureLink = _storageProvider.GetDefaultOrBlobUrl(userProfile)
            });

            return View(viewmodel);
        }
    }
}
