using Marten;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Projects;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class TestDataSeeder
    {
        private readonly IDocumentSession _documentSession;
        private readonly TestUserSeeder _userSeeder;
        private readonly TestProjectSeeder _projectSeeder;

        public TestDataSeeder(IDocumentSession documentSession, TestUserSeeder userSeeder, TestProjectSeeder projectSeeder)
        {
            _documentSession = documentSession;
            _userSeeder = userSeeder;
            _projectSeeder = projectSeeder;
        }

        internal async Task SeedIfNecessary()
        {
            var isSeedNecessary = !_documentSession.Query<Project>().Any();
            if (!isSeedNecessary)
            {
                return;
            }

            var user = await _userSeeder.SeedUser();
            _projectSeeder.SeedProject(user);
        }
    }
}
