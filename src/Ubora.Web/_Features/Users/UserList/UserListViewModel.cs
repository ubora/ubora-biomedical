using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;

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

            protected Factory()
            {
            }

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public virtual IEnumerable<UserListItemViewModel> GetUserListItemViewModels()
            {
                var users = _queryProcessor.Find<UserProfile>()
                    .Select(x => new UserListItemViewModel
                    {
                        UserId = x.UserId,
                        FullName = x.FullName,
                        Email = x.Email
                    });
                return users;
            }
        }
    }
}
