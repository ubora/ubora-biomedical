using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class IsViewed<T> : Specification<T> where T : BaseNotification
    {
        public IsViewed()
        {
        }

        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.HasBeenViewed;
        }
    }

    public class IsViewed : IsViewed<BaseNotification>
    {
    }
}
