using Marten.Pagination;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Users.Queries
{
    public class SearchUboraMentorUserProfilesQuery : IQuery<IPagedList<UserProfile>>
    {
        public SearchUboraMentorUserProfilesQuery(ISortSpecification<UserProfile> sortSpecification, int pageSize, int pageNumber)
        {
            SortSpecification = sortSpecification;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public ISortSpecification<UserProfile> SortSpecification { get; }
        public int PageSize { get; }
        public int PageNumber { get; }
    }
}
