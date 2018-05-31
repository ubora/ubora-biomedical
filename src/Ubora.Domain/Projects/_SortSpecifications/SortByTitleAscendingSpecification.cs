using System.Linq;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects._SortSpecifications
{
    public class SortByTitleAscendingSpecification : ISortSpecification<Project>
    {
        public IQueryable<Project> Sort(IQueryable<Project> query)
        {
            return query.OrderBy(l => l.Title);
        }
    }
}
