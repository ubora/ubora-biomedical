using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Users.UserList
{
    public class UserListController : Controller
    {
        private readonly UserListViewModel.Factory _modelFactory;

        public UserListController(UserListViewModel.Factory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public IActionResult Index()
        {
            var users = _modelFactory.GetUserListItemViewModels();

            return View(users);
        }
    }
}
