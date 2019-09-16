using Marten;
using System.Linq;
using System.Threading.Tasks;
using Marten.Linq.SoftDeletes;
using Ubora.Domain.Projects;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class TestDataSeeder
    {
        private readonly IDocumentSession _documentSession;
        private readonly TestUserSeeder _userSeeder;
        private readonly TestProjectSeeder _projectSeeder;
        private readonly TestMentorSeeder _mentorSeeder;
        private readonly TestClinicalNeedSeeder _clinicalNeedSeeder;

        public TestDataSeeder(IDocumentSession documentSession, 
            TestUserSeeder userSeeder, 
            TestProjectSeeder projectSeeder,
            TestMentorSeeder mentorSeeder,
            TestClinicalNeedSeeder clinicalNeedSeeder)
        {
            _documentSession = documentSession;
            _userSeeder = userSeeder;
            _projectSeeder = projectSeeder;
            _mentorSeeder = mentorSeeder;
            _clinicalNeedSeeder = clinicalNeedSeeder;
        }

        internal async Task SeedIfNecessary()
        {
            var isAlreadySeeded = _documentSession.Query<Project>().Any(p => p.MaybeDeleted());
            if (isAlreadySeeded)
            {
                return;
            }

            var user = await _userSeeder.SeedUser();
            _projectSeeder.SeedProject(user);

            for (int i = 0; i < 22; i++)
            {
                _clinicalNeedSeeder.SeedClinicalNeed(user);
            }

            await _mentorSeeder.SeedMentor();
        }
    }
}
