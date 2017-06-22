using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class UnViewedNotifications<T> : Specification<T> where T : BaseNotification
    {
        private Guid _userId;

        public UnViewedNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId && !x.HasBeenViewed;
        }
    }

    public class UnViewedNotifications : UnViewedNotifications<BaseNotification>
    {
        public UnViewedNotifications(Guid userId) : base(userId)
        {
        }
    }
}
