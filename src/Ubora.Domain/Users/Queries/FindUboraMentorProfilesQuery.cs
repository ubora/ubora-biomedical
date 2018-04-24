using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Users.Queries
{
    public class FindUboraMentorProfilesQuery : IQuery<IReadOnlyCollection<UserProfile>>
    {
    }
}