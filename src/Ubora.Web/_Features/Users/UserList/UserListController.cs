using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;

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
            var userProfiles = QueryProcessor.Find<UserProfile>()
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
    }
}
