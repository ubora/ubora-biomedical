using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class HasArchivedNotifications<T> : Specification<T> where T : BaseNotification
    {
        protected Guid _userId;
        public HasArchivedNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId && x.IsArchived;
        }
    }

    public class HasArchivedNotifications : HasArchivedNotifications<BaseNotification>
    {
        public HasArchivedNotifications(Guid userId) : base(userId)
        {
        }
    }
}
