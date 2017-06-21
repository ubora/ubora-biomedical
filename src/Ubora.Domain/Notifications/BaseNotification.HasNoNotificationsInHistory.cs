using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class HasNoNotificationsInHistory : Specification<BaseNotification>
    {
        protected Guid _userId;
        public HasNoNotificationsInHistory(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<BaseNotification, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId && !x.InHistory;
        }
    }
}
