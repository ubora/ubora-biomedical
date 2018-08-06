using Marten.Pagination;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Users.Queries
{
    public class SortByMultipleUboraMentorProfilesQuery : IQuery<IPagedList<UserProfile>>
    {
        public SortByMultipleUboraMentorProfilesQuery(List<ISortSpecification<UserProfile>> sortSpecifications, int pageSize, int pageNumber)
        {
            SortSpecifications = sortSpecifications;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public List<ISortSpecification<UserProfile>> SortSpecifications { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
