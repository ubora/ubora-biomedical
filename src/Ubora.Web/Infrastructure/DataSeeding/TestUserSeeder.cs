﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class TestUserSeeder
    {
        private readonly ICommandProcessor _processor;
        private readonly ApplicationUserManager _userManager;

        public TestUserSeeder(ICommandProcessor processor, ApplicationUserManager userManager)
        {
            _processor = processor;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> SeedUser()
        {
            var email = "test@agileworks.eu";
            var password = "ChangeMe123!";

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, password);
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
