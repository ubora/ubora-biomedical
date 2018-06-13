using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.SortSpecifications
{
    public class SortByCreatedAtSpecification : ISortSpecification<INotification>
    {
        public SortOrder SortOrder { get; }

        public SortByCreatedAtSpecification(SortOrder sortOrder = SortOrder.Ascending)
        {
            SortOrder = sortOrder;
        }

        public IQueryable<INotification> Sort(IQueryable<INotification> query)
        {
            switch (SortOrder)
            {
                case SortOrder.Ascending:
                    return query.OrderBy(l => l.CreatedAt);
                case SortOrder.Descending:
                    return query.OrderByDescending(l => l.CreatedAt);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}