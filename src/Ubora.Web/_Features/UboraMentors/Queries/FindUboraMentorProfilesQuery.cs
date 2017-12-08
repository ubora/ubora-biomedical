using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Members.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.UboraMentors.Queries
{
    public class FindUboraMentorProfilesQuery : IQuery<IReadOnlyCollection<UserProfile>>
    {
        public class Handler : IQueryHandler<FindUboraMentorProfilesQuery, IReadOnlyCollection<UserProfile>>
        {
            private readonly ApplicationUserManager _userManager;
            private readonly IQueryProcessor _queryProcessor;

            public Handler(ApplicationUserManager userManager, IQueryProcessor queryProcessor)
            {
                _userManager = userManager;
                _queryProcessor = queryProcessor;
            }

            public IReadOnlyCollection<UserProfile> Handle(FindUboraMentorProfilesQuery query)
            {
                var mentorUserIds = _userManager.GetUsersInRoleAsync(ApplicationRole.Mentor)
                    .GetAwaiter().GetResult()
                    .Select(x => x.Id);

                var uboraMentors = _queryProcessor.ExecuteQuery(new FindUserProfilesQuery
                {
                    UserIds = mentorUserIds
                });

                return uboraMentors;
            }
        }
    }
}