using System;
using Marten;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web
{
    public class Seeder
    {
        private readonly IDocumentSession _documentSession;
        private readonly ApplicationUserManager _userManager;
        private readonly IConfigurationRoot _configuration;
        private readonly ApplicationRoleManager _roleManager;
        private readonly ICommandQueryProcessor _processor;

        public Seeder(IDocumentSession documentSession, ApplicationUserManager userManager, IConfigurationRoot configuration, ApplicationRoleManager roleManager, ICommandQueryProcessor processor)
        {
            _documentSession = documentSession;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _processor = processor;
        }

        internal void SeedIfNecessary()
        {
            var isSeedNecessary = !_documentSession.Query<DeviceClassification>().Any();
            if (isSeedNecessary)
            {
                var deviceClassification = new DeviceClassification();
                deviceClassification.CreateNew();

                _documentSession.Store(deviceClassification);
                _documentSession.SaveChanges();

                AddRolesIfNecessary().Wait();
                SeedAdmin().Wait();
            }
        }

        private async Task SeedAdmin()
        {
            var userName = _configuration["AdminUser:UserName"];
            var password = _configuration["AdminUser:Password"];
            var admin = new ApplicationUser
            {
                UserName = userName,
                Email = userName
            };
            var result = await _userManager.CreateAsync(admin, password);

            _processor.Execute(new CreateUserProfileCommand
            {
                UserId = admin.Id,
                Email = admin.Email,
                FirstName = "System",
                LastName = "Administrator"
            });

            await _userManager.AddToRoleAsync(admin, ApplicationRole.Admin);
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
