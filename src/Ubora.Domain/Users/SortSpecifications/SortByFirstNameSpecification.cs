using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Users.SortSpecifications
{
    public class SortByFirstNameSpecification : ISortSpecification<UserProfile>
    {
        public SortOrder SortOrder { get; }

        public SortByFirstNameSpecification(SortOrder sortOrder = SortOrder.Ascending)
        {
            SortOrder = sortOrder;
        }

        public IQueryable<UserProfile> Sort(IQueryable<UserProfile> query)
        {
            switch (SortOrder)
            {
                case SortOrder.Ascending:
                    return query.OrderBy(l => l.FirstName);
                case SortOrder.Descending:
                    return query.OrderByDescending(l => l.FirstName);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
