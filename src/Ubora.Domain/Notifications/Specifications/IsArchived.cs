using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class IsArchived<T> : Specification<T> where T : BaseNotification
    {
        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.IsArchived == true;
        }
    }

    public class IsArchived : IsArchived<BaseNotification>
    {
    }
}
