using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Members.Queries;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;
using Ubora.Web.Data;

namespace Ubora.Web.Services
{
    public class FindUboraAdministratorsQueryHandler : IQueryHandler<FindUboraAdministratorsQuery, IReadOnlyCollection<UserProfile>>
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IQueryProcessor _queryProcessor;

        public FindUboraAdministratorsQueryHandler(IApplicationUserManager userManager, IQueryProcessor queryProcessor)
        {
            _userManager = userManager;
            _queryProcessor = queryProcessor;
        }

        public IReadOnlyCollection<UserProfile> Handle(FindUboraAdministratorsQuery query)
        {
            var adminUserIds = _userManager.GetUsersInRoleAsync(ApplicationRole.Admin)
                .GetAwaiter().GetResult()
                .Select(x => x.Id);

            var uboraAdmins = _queryProcessor.ExecuteQuery(new FindUserProfilesQuery
            {
                UserIds = adminUserIds
            });

            return uboraAdmins;
        }
    }
}
