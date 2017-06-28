using System;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class HasArchivedNotifications<T> : WrappedSpecification<T> where T : BaseNotification
    {
        private Guid _userId;

        public HasArchivedNotifications(Guid userId)
        {
            _userId = userId;
        }

        public override Specification<T> Specification => new IsForUser<T>(_userId) && new IsArchived<T>();
    }

    public class HasArchivedNotifications : HasArchivedNotifications<BaseNotification>
    {
        public HasArchivedNotifications(Guid userId) : base(userId)
        {
        }
    }
}
