using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class IsForUser<T> : Specification<T> where T : BaseNotification
    {
        protected Guid _userId;
        public IsForUser(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.NotificationTo == _userId;
        }
    }

    public class IsForUser : IsForUser<BaseNotification>
    {
        public IsForUser(Guid userId) : base(userId)
        {
        }
    }
}
