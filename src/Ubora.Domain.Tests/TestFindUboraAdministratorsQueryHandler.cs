using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;

namespace Ubora.Domain.Tests
{
    public class TestFindUboraAdministratorsQueryHandler : IQueryHandler<FindUboraAdministratorsQuery, IReadOnlyCollection<UserProfile>>
    {
        public IReadOnlyCollection<UserProfile> AdministratorsProfilesToReturn { get; set; } = new List<UserProfile>();

        public IReadOnlyCollection<UserProfile> Handle(FindUboraAdministratorsQuery query)
        {
            return AdministratorsProfilesToReturn;
        }
    }
}
