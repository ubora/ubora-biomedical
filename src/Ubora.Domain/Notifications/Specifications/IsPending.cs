using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class IsPending<T> : Specification<T> where T : BaseNotification
    {
        public IsPending()
        {
        }

        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.IsPending;
        }
    }

    public class IsPending : IsPending<BaseNotification>
    {
    }
}
