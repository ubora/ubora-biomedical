using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class HasNotificationsInHistory : Specification<BaseNotification>
    {
        protected Guid _userId;
        public HasNotificationsInHistory(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<BaseNotification, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId && x.InHistory;
        }
    }
}
