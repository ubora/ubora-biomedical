using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class IsPending<T> : Specification<T> where T : INotification
    {
        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => !x.IsArchived;
        }
    }

    public class IsPending : IsPending<INotification>
    {
    }
}
