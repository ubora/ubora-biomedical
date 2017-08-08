using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class TestUserSeeder
    {
        public class Options
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        private readonly Options _options;
        private readonly ICommandProcessor _processor;
        private readonly ApplicationUserManager _userManager;

        public TestUserSeeder(IOptions<Options> optionsAccessor, ICommandProcessor processor, ApplicationUserManager userManager)
        {
            _options = optionsAccessor.Value;
            _processor = processor;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> SeedUser()
        {
            var user = await CreateUserAndProfile();
            return user;
        }

        private async Task<ApplicationUser> CreateUserAndProfile()
        {
            var user = new ApplicationUser
            {
                UserName = _options.UserName,
                Email = _options.UserName
            };

            var identityResult = await _userManager.CreateAsync(user, _options.Password);
            if (!identityResult.Succeeded)
            {
                var errorMessages = identityResult.Errors.Select(x => x.Description);
                throw new Exception($"User account could not be created because: {string.Join(",", errorMessages)}");
            }

            var commandResult = _processor.Execute(new CreateUserProfileCommand
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = "Test",
                LastName = "User"
            });
            if (commandResult.IsFailure)
            {
                throw new Exception($"User profile could not be created because: {string.Join(",", commandResult.ErrorMessages)}");
            }

            return user;
        }
    }
}
