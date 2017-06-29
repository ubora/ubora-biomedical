

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Users.UserList
{
    public class UserListController : UboraController
    {
        private readonly IStorageProvider _storageProvider;

        public UserListController(ICommandQueryProcessor processor, IStorageProvider storageProvider) : base(processor)
        {
            _storageProvider = storageProvider;
        }

        public IActionResult Index()
        {
            var userProfiles = Find<UserProfile>();

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
