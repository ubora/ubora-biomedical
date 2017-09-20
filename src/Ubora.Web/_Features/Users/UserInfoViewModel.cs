using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Web._Features.Users
{
    public class UserInfoViewModel
    {
        public Guid UserId { get; private set; }
        public string Name { get; private set; }

        public UserInfoViewModel(UserInfo userInfo)
        {
            UserId = userInfo.UserId;
            Name = userInfo.Name;
        }
    }
}
