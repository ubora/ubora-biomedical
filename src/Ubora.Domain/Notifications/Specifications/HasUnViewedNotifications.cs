using System;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class HasUnViewedNotifications<T> : WrappedSpecification<T> where T : BaseNotification
    {
        private readonly Guid _userId;

        public HasUnViewedNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Specification<T> WrapSpecifications()
        {
            return new IsForUser<T>(_userId) && !new IsViewed<T>();
        }
    }

    public class HasUnViewedNotifications : HasUnViewedNotifications<BaseNotification>
    {
        public HasUnViewedNotifications(Guid userId) : base(userId)
        {
        }
    }
}
