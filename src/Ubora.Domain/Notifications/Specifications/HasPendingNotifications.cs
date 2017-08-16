using System;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class HasPendingNotifications<T> : WrappedSpecification<T> where T : INotification
    {
        public Guid UserId { get; }

        public HasPendingNotifications(Guid userId)
        {
            UserId = userId;
        }

        internal override Specification<T> WrapSpecifications()
        {
            return new IsForUser<T>(UserId) && new IsPending<T>(); ;
        }
    }

    public class HasPendingNotifications : HasPendingNotifications<INotification>
    {
        public HasPendingNotifications(Guid userId) : base(userId)
        {
        }
    }
}
