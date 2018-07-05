using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Members.Queries;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;
using Ubora.Web.Data;

namespace Ubora.Web.Services
{
    public class FindUboraManagementGroupQueryHandler : IQueryHandler<FindUboraManagementGroupQuery, IReadOnlyCollection<UserProfile>>
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IQueryProcessor _queryProcessor;

        public FindUboraManagementGroupQueryHandler(IApplicationUserManager userManager, IQueryProcessor queryProcessor)
        {
            _userManager = userManager;
            _queryProcessor = queryProcessor;
        }

        public IReadOnlyCollection<UserProfile> Handle(FindUboraManagementGroupQuery query)
        {
            var managementGroupUserIds = _userManager.GetUsersInRoleAsync(ApplicationRole.ManagementGroup)
                .GetAwaiter().GetResult()
                .Select(x => x.Id);

            var uboraManagementGroups = _queryProcessor.ExecuteQuery(new FindUserProfilesQuery
            {
                UserIds = managementGroupUserIds
            });

            return uboraManagementGroups;
        }
    }
}
