using System.Linq;
using System.Threading.Tasks;
using Marten;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class ApplicationDataSeeder
    {
        private readonly IDocumentSession _documentSession;
        private readonly ApplicationRoleManager _roleManager;
        private readonly AdminSeeder _adminSeeder;

        public ApplicationDataSeeder(IDocumentSession documentSession, ApplicationRoleManager roleManager, AdminSeeder adminSeeder)
        {
            _documentSession = documentSession;
            _roleManager = roleManager;
            _adminSeeder = adminSeeder;
        }

        internal async Task SeedIfNecessary()
        {
            await AddRoleIfNecessary(ApplicationRole.Admin);
            await AddRoleIfNecessary(ApplicationRole.Mentor);

            var isSeedNecessary = !_documentSession.Query<UserProfile>().Any();
            if (!isSeedNecessary)
            {
                return;
            }

            await _adminSeeder.SeedAdmin();
        }

        private async Task AddRoleIfNecessary(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = roleName
                });
            }
        }
    }
}
