using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects._SortSpecifications
{
    public class SortByCreatedDateTimeSpecfication : ISortSpecification<Project>
    {
        public SortOrder SortOrder { get; }

        public SortByCreatedDateTimeSpecfication(SortOrder sortOrder = SortOrder.Ascending)
        {
            SortOrder = sortOrder;
        }

        public IQueryable<Project> Sort(IQueryable<Project> query)
        {
            switch (SortOrder)
            {
                case SortOrder.Ascending:
                    return query.OrderBy(l => l.CreatedDateTime);
                case SortOrder.Descending:
                    return query.OrderByDescending(l => l.CreatedDateTime);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
