using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class HasPendingNotifications<T> : Specification<T> where T:BaseNotification
    {
        protected Guid _userId;
        public HasPendingNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId && x.IsPending;
        }
    }

    public class HasPendingNotifications : HasPendingNotifications<BaseNotification>
    {
        public HasPendingNotifications(Guid userId) : base(userId)
        {
        }
    }
}
