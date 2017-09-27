using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class AdminSeeder
    {
        public class Options
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        private readonly Options _options;
        private readonly ICommandProcessor _processor;
        private readonly ApplicationUserManager _userManager;

        public AdminSeeder(IOptions<Options> optionsAccessor, ICommandProcessor processor, ApplicationUserManager userManager)
        {
            _options = optionsAccessor.Value;
            _processor = processor;
            _userManager = userManager;
        }

        public async Task SeedAdmin()
        {
            var adminUser = await CreateAdminUserAndProfile();

            await AddAdminRole(adminUser);
        }

        private async Task<ApplicationUser> CreateAdminUserAndProfile()
        {
            var user = new ApplicationUser
            {
                UserName = _options.UserName,
                Email = _options.UserName,
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, _options.Password);
            if (!identityResult.Succeeded)
            {
                var errorMessages = identityResult.Errors.Select(x => x.Description);
                throw new Exception($"Admin account could not be created because: {string.Join(",", errorMessages)}");
            }

            var commandResult = _processor.Execute(new CreateUserProfileCommand
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = "System",
                LastName = "Administrator"
            });
            if (commandResult.IsFailure)
            {
                throw new Exception($"Admin user profile could not be created because: {string.Join(",", commandResult.ErrorMessages)}");
            }

            return user;
        }

        private async Task AddAdminRole(ApplicationUser adminUser)
        {
            var identityResult = await _userManager.AddToRoleAsync(adminUser, ApplicationRole.Admin);
            if (!identityResult.Succeeded)
            {
                var errorMessages = identityResult.Errors.Select(x => x.Description);
                throw new Exception($"Admin role could not be added because: {string.Join(",", errorMessages)}");
            }
        }
    }
}