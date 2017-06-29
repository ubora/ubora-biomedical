using System;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class HasPendingNotifications<T> : WrappedSpecification<T> where T : BaseNotification
    {
        private readonly Guid _userId;

        public HasPendingNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Specification<T> WrapSpecifications()
        {
            return new IsForUser<T>(_userId) && new IsPending<T>(); ;
        }
    }

    public class HasPendingNotifications : HasPendingNotifications<BaseNotification>
    {
        public HasPendingNotifications(Guid userId) : base(userId)
        {
        }
    }
}
