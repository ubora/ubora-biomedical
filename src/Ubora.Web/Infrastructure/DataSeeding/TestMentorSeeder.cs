using System;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class TestMentorSeeder
    {
        private readonly ICommandProcessor _processor;
        private readonly ApplicationUserManager _userManager;

        public TestMentorSeeder(ICommandProcessor processor, ApplicationUserManager userManager)
        {
            _processor = processor;
            _userManager = userManager;
        }

        public async Task SeedMentor()
        {
            var mentorUser = await CreateMentorUserAndProfile();

            await AddMentorRole(mentorUser);
        }

        private async Task<ApplicationUser> CreateMentorUserAndProfile()
        {
            var user = new ApplicationUser
            {
                UserName = "mentor@agileworks.eu",
                Email = "mentor@agileworks.eu",
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, "ChangeMe123!");
            if (!identityResult.Succeeded)
            {
                var errorMessages = identityResult.Errors.Select(x => x.Description);
                throw new Exception($"Mentor account could not be created because: {string.Join(",", errorMessages)}");
            }

            var commandResult = _processor.Execute(new CreateUserProfileCommand
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = "Test",
                LastName = "Mentor"
            });
            if (commandResult.IsFailure)
            {
                throw new Exception($"Mentor user profile could not be created because: {string.Join(",", commandResult.ErrorMessages)}");
            }

            return user;
        }

        private async Task AddMentorRole(ApplicationUser mentorUser)
        {
            var identityResult = await _userManager.AddToRoleAsync(mentorUser, ApplicationRole.Mentor);
            if (!identityResult.Succeeded)
            {
                var errorMessages = identityResult.Errors.Select(x => x.Description);
                throw new Exception($"Mentor role could not be added because: {string.Join(",", errorMessages)}");
            }
        }
    }
}
