using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Users
{
    // TODO(Kaspar Kallas): Completely alpha atm
    public class UsersController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;

        public UsersController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public IActionResult Index()
        {
            var users = _queryProcessor.Find<UserProfile>()
                .Select(x => new UserListItemViewModel
            {
                UserId = x.UserId,
                FullName = x.FullName
            });

            return View(users);
        }
    }

    public class UserListItemViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
    }
}
