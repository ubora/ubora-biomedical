using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects._SortSpecifications
{
    public class SortByMultipleSpecification<T> : ISortSpecification<T>
    {
        public List<ISortSpecification<T>> SortSpecifications { get; set; }

        public SortByMultipleSpecification(List<ISortSpecification<T>> sortSpecifications)
        {
            SortSpecifications = sortSpecifications;
        }

        public IQueryable<T> Sort(IQueryable<T> query)
        {
            foreach (var sortSpec in SortSpecifications)
            {
                query = sortSpec.Sort(query);
            }

            return query;
        }
    }
}
