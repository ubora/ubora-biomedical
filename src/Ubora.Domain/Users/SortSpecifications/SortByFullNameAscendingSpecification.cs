using System.Linq;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Users.SortSpecifications
{
    public class SortByFullNameAscendingSpecification : ISortSpecification<UserProfile>
    {
        public IQueryable<UserProfile> Sort(IQueryable<UserProfile> query)
        {
            return query.OrderBy(l => l.FullName);
        }
    }
}
