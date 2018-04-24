using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;

namespace Ubora.Domain.Tests
{
    public class TestFindUboraMentorProfilesQueryHandler : IQueryHandler<FindUboraMentorProfilesQuery, IReadOnlyCollection<UserProfile>>
    {
        public IReadOnlyCollection<UserProfile> UserMentorProfilesToReturn { get; set; } = new List<UserProfile>();
            
        public IReadOnlyCollection<UserProfile> Handle(FindUboraMentorProfilesQuery query)
        {
            return UserMentorProfilesToReturn;
        }
    }
}