using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Users.UserList
{
    public class UserListViewModel
    {
        public class UserListItemViewModel
        {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
        }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly UserManager<ApplicationUser> _userManager;

            public Factory(IQueryProcessor queryProcessor, UserManager<ApplicationUser> userManager)
            {
                _queryProcessor = queryProcessor;
                _userManager = userManager;
            }

            public IEnumerable<UserListItemViewModel> GetUserListItemViewModels()
            {
                var users = _queryProcessor.Find<UserProfile>()
                    .Select(x => new UserListItemViewModel
                    {
                        UserId = x.UserId,
                        FullName = x.FullName,
                        Email = _userManager.FindByIdAsync(x.UserId.ToString()).Result.Email
                    });
                return users;
            }
        }
    }
}
