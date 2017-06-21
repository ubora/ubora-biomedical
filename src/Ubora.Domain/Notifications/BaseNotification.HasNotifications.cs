using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class HasNotifications<T> : Specification<T> where T:BaseNotification
    {
        protected Guid _userId;
        public HasNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId;
        }
    }

    public class HasNotifications : HasNotifications<BaseNotification>
    {
        public HasNotifications(Guid userId) : base(userId)
        {
        }
    }
}
