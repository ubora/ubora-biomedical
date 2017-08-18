using System;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class HasUnViewedNotifications<T> : WrappedSpecification<T> where T : INotification
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

    public class HasUnViewedNotifications : HasUnViewedNotifications<INotification>
    {
        public HasUnViewedNotifications(Guid userId) : base(userId)
        {
        }
    }
}
