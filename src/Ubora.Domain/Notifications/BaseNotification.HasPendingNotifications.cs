using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class HasPendingNotifications : Specification<BaseNotification>
    {
        protected Guid _userId;
        public HasPendingNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<BaseNotification, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId && x.IsPending;
        }
    }
}
