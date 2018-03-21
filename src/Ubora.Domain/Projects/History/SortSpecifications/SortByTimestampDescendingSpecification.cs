using System.Linq;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects.History.SortSpecifications
{
    public class SortByTimestampDescendingSpecification : ISortSpecification<EventLogEntry>
    {
        public IQueryable<EventLogEntry> Sort(IQueryable<EventLogEntry> query)
        {
            return query.OrderByDescending(l => l.Timestamp);
        }
    }
}
