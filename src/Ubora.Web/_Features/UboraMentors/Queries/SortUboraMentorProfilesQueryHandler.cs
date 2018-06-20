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
    public class SortByMultipleUboraMentorProfilesQueryHandler : IQueryHandler<SortByMultipleUboraMentorProfilesQuery, IPagedList<UserProfile>>
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IQuerySession _querySession;

        public SortByMultipleUboraMentorProfilesQueryHandler(IApplicationUserManager userManager, IQuerySession querySession)
        {
            _userManager = userManager;
            _querySession = querySession;
        }

        public IPagedList<UserProfile> Handle(SortByMultipleUboraMentorProfilesQuery query)
        {
            var mentorUserIds = _userManager.GetUsersInRoleAsync(ApplicationRole.Mentor)
                .GetAwaiter().GetResult()
                .Select(x => x.Id);

            var uboraMentors = _querySession.Query<UserProfile>().Where(userProfile => userProfile.UserId.IsOneOf(mentorUserIds.ToArray()));

            foreach (var sortSpec in query.SortSpecifications)
            {
                uboraMentors = sortSpec.Sort(uboraMentors);
            }

            return uboraMentors.AsPagedList(query.PageNumber, query.PageSize);
        }
    }
}
