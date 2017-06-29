using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class HasUnViewedNotifications<T> : WrappedSpecification<T> where T : BaseNotification
    {
        private Guid _userId;

        public HasUnViewedNotifications(Guid userId)
        {
            _userId = userId;
        }

        public override Specification<T> Specification => new IsForUser<T>(_userId) && !new IsViewed<T>();
    }

    public class HasUnViewedNotifications : HasUnViewedNotifications<BaseNotification>
    {
        public HasUnViewedNotifications(Guid userId) : base(userId)
        {
        }
    }
}
