using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;

namespace Ubora.Domain.Tests
{
    public class TestFindUboraManagementGroupHandler : IQueryHandler<FindUboraManagementGroupQuery, IReadOnlyCollection<UserProfile>>
    {
        public IReadOnlyCollection<UserProfile> UboraManagementGroupProfilesToReturn { get; set; } = new List<UserProfile>();

        public IReadOnlyCollection<UserProfile> Handle(FindUboraManagementGroupQuery query)
        {
            return UboraManagementGroupProfilesToReturn;
        }
    }
}
