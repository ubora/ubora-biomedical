using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects._SortSpecifications
{
    public class SortByMultipleProjectSpecification : ISortSpecification<Project>
    {
        public List<ISortSpecification<Project>> SortSpecifications { get; set; }

        public SortByMultipleProjectSpecification(List<ISortSpecification<Project>> sortSpecifications)
        {
            SortSpecifications = sortSpecifications;
        }

        public IQueryable<Project> Sort(IQueryable<Project> query)
        {
            foreach (var sortSpec in SortSpecifications)
            {
                query = sortSpec.Sort(query);
            }

            return query;
        }
    }
}
