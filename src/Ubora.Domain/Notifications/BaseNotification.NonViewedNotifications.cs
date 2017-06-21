using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class NonViewedNotifications<T> : Specification<T> where T:BaseNotification
    {
        private Guid _userId;

        public NonViewedNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId && !x.HasBeenViewed;
        }
    }

    public class NonViewedNotifications : NonViewedNotifications<BaseNotification>
    {
        public NonViewedNotifications(Guid userId) : base(userId)
        {
        }
    }
}
