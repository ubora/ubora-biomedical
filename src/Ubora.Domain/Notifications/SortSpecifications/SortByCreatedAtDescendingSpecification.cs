using System.Linq;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Notifications.SortSpecifications
{
    public class SortByCreatedAtDescendingSpecification : ISortSpecification<INotification>
    {
        public IQueryable<INotification> Sort(IQueryable<INotification> query)
        {
            return query.OrderByDescending(l => l.CreatedAt);
        }
    }
}
