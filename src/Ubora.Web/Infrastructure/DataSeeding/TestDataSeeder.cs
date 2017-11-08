using Marten;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Users;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class TestDataSeeder
    {
        private readonly IDocumentSession _documentSession;
        private readonly TestUserSeeder _userSeeder;
        private readonly TestProjectSeeder _projectSeeder;
        private readonly TestMentorSeeder _mentorSeeder;

        public TestDataSeeder(IDocumentSession documentSession, 
            TestUserSeeder userSeeder, 
            TestProjectSeeder projectSeeder,
            TestMentorSeeder mentorSeeder)
        {
            _documentSession = documentSession;
            _userSeeder = userSeeder;
            _projectSeeder = projectSeeder;
            _mentorSeeder = mentorSeeder;
        }

        internal async Task SeedIfNecessary()
        {
            var isSeedNecessary = !_documentSession.Query<UserProfile>().Any();
            if (!isSeedNecessary)
            {
                return;
            }

            var user = await _userSeeder.SeedUser();
            _projectSeeder.SeedProject(user);

            await _mentorSeeder.SeedMentor();
        }
    }
}
