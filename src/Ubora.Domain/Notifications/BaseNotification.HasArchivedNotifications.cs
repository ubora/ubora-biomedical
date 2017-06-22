using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class HasArchivedNotifications : Specification<BaseNotification>
    {
        protected Guid _userId;
        public HasArchivedNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<BaseNotification, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId && x.IsArchived;
        }
    }
}
