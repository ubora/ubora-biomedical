using System.Linq;
using System.Threading.Tasks;
using Marten;
using Ubora.Domain.Projects.DeviceClassification;
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
            await AddRolesIfNecessary();

            var isSeedNecessary = !_documentSession.Query<DeviceClassification>().Any();
            if (!isSeedNecessary)
            {
                return;
            }

            var deviceClassification = new DeviceClassification();
            deviceClassification.CreateNew();

            _documentSession.Store(deviceClassification);
            _documentSession.SaveChanges();

            await _adminSeeder.SeedAdmin();
        }

        private async Task AddRolesIfNecessary()
        {
            if (!await _roleManager.RoleExistsAsync(ApplicationRole.Admin))
            {
                var adminRole = new ApplicationRole
                {
                    Name = ApplicationRole.Admin
                };
                await _roleManager.CreateAsync(adminRole);
            }
        }
    }
}
