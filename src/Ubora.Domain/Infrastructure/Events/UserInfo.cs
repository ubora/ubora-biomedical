using System;

namespace Ubora.Domain.Infrastructure.Events
{
    public class UserInfo
    {
        public UserInfo(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public Guid UserId { get; private set; }
        public string Name { get; private set; }
    }
}