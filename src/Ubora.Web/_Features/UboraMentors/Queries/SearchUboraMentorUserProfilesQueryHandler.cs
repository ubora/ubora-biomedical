using Marten;
using Marten.Pagination;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.UboraMentors.Queries
{
    public class SearchUboraMentorUserProfilesQueryHandler : IQueryHandler<SearchUboraMentorUserProfilesQuery, IPagedList<UserProfile>>
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IQuerySession _querySession;

        public SearchUboraMentorUserProfilesQueryHandler(IApplicationUserManager userManager, IQuerySession querySession)
        {
            _userManager = userManager;
            _querySession = querySession;
        }

        public IPagedList<UserProfile> Handle(SearchUboraMentorUserProfilesQuery query)
        {
            var mentorUserIds = _userManager.GetUsersInRoleAsync(ApplicationRole.Mentor)
                .GetAwaiter().GetResult()
                .Select(x => x.Id);

            var uboraMentorUserProfiles = _querySession.Query<UserProfile>()
                .Where(userProfile => userProfile.UserId.IsOneOf(mentorUserIds.ToArray()))
                .Sort(query.SortSpecification);

            return uboraMentorUserProfiles.AsPagedList(query.PageNumber, query.PageSize);
        }
    }
}
