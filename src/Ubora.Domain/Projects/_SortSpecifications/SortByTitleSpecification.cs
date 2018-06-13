using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects._SortSpecifications
{
    public class SortByTitleSpecification : ISortSpecification<Project>
    {
        public SortOrder SortOrder { get; }

        public SortByTitleSpecification(SortOrder sortOrder = SortOrder.Ascending)
        {
            SortOrder = sortOrder;
        }

        public IQueryable<Project> Sort(IQueryable<Project> query)
        {
            switch (SortOrder)
            {
                case SortOrder.Ascending:
                    return query.OrderBy(l => l.Title);
                case SortOrder.Descending:
                    return query.OrderByDescending(l => l.Title);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
