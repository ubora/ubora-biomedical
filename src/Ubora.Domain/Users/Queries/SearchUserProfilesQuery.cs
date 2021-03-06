﻿using Marten;
using Marten.Pagination;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Users.SortSpecifications;
using Ubora.Domain.Users.Specifications;

namespace Ubora.Domain.Users.Queries
{
    public class SearchUserProfilesQuery : IQuery<IPagedList<UserProfile>>
    {
        public string SearchFullName { get; set; }
        public Paging Paging { get; set; }
        public Specification<UserProfile> WhereSpecification { get; set; } = new MatchAll<UserProfile>();
        public ISortSpecification<UserProfile> SortSpecification { get; set; } = new SortByFullNameAscendingSpecification();

        public class Handler : IQueryHandler<SearchUserProfilesQuery, IPagedList<UserProfile>>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public IPagedList<UserProfile> Handle(SearchUserProfilesQuery query)
            {
                var martenPagedList = _querySession.Query<UserProfile>()
                                         .Where(query.WhereSpecification)
                                         .Where(new UserFullNameContainsPhraseSpec(query.SearchFullName))
                                         .Sort(query.SortSpecification)
                                         .AsPagedList(query.Paging.PageNumber, query.Paging.PageSize);

                return new Infrastructure.Queries.PagedList<UserProfile>(martenPagedList.ToList(), martenPagedList);
            }
        }
    }
}
