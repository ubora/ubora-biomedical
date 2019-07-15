using Marten.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Users;
using Ubora.Web._Features._Shared.Paging;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Admin
{
    public class ManageUsersViewModel
    {
        public Pager Pager { get; set; }
        public List<UserViewModel> Items { get; set; }
        public string SearchName { get; set; }

        public class Factory
        {
            private readonly IApplicationUserManager _userManager;

            public Factory(IApplicationUserManager userManager, ApplicationDbContext dbContext)
            {
                _userManager = userManager;
                var users = dbContext.Users.ToList();
            }

            protected Factory()
            {
            }

            public virtual async Task<ManageUsersViewModel> Create(IReadOnlyCollection<UserProfile> userProfiles, string searchName, Pager pager)
            {
                var userIds = userProfiles.Select(x => x.UserId).ToList();
                var applicationUsers = _userManager.Users
                    .Where(u => userIds.Contains(u.Id))
                    .ToDictionary(u => u.Id, u => u);

                var userViewModels = new List<UserViewModel>();
                foreach (var userProfile in userProfiles)
                {
                    userViewModels.Add(new UserViewModel
                    {
                        UserId = userProfile.UserId,
                        UserEmail = userProfile.Email,
                        FullName = userProfile.FullName,
                        Roles = await _userManager.GetRolesAsync(applicationUsers[userProfile.UserId])
                    });
                }

                var manageUsersViewModel = new ManageUsersViewModel
                {
                    Pager = pager,
                    Items = userViewModels,
                    SearchName = searchName
                };

                return manageUsersViewModel;
            }
        }
    }
}
