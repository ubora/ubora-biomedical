using System;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class HasPendingNotifications<T> : WrappedSpecification<T> where T : BaseNotification
    {
        protected Guid _userId;
        public HasPendingNotifications(Guid userId)
        {
            _userId = userId;
        }

        public override Specification<T> Specification => new IsForUser<T>(_userId) && new IsPending<T>();
    }

    public class HasPendingNotifications : HasPendingNotifications<BaseNotification>
    {
        public HasPendingNotifications(Guid userId) : base(userId)
        {
        }
    }
}
