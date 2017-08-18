using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class IsForUser<T> : Specification<T> where T : INotification
    {
        public Guid UserId { get; }

        public IsForUser(Guid userId)
        {
            UserId = userId;
        }

        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.NotificationTo == UserId;
        }
    }

    public class IsForUser : IsForUser<INotification>
    {
        public IsForUser(Guid userId) : base(userId)
        {
        }
    }
}
