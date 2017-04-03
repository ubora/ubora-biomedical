using System;

namespace Ubora.Domain.Events
{
    public abstract class UboraEvent
    {
        protected UboraEvent(UserInfo creator)
        {
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
        }
        public UserInfo Creator { get; }

        public abstract string Description();

        public override string ToString()
        {
            return $"\"{Creator.Name}\": {Description()}";
        }
    }

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
