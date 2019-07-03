using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Users.Queries
{
    public class FindUboraAdministratorsQuery : IQuery<IReadOnlyCollection<UserProfile>>
    {
    }
}
