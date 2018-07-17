using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Users.SortSpecifications
{
    public class SortByMultipleUserProfileSortSpecification : ISortSpecification<UserProfile>
    {
        public List<ISortSpecification<UserProfile>> SortSpecifications { get; set; }

        public SortByMultipleUserProfileSortSpecification(List<ISortSpecification<UserProfile>> sortSpecifications)
        {
            SortSpecifications = sortSpecifications;
        }

        public IQueryable<UserProfile> Sort(IQueryable<UserProfile> query)
        {
            foreach (var sortSpec in SortSpecifications)
            {
                query = sortSpec.Sort(query);
            }

            return query;
        }
    }
}
