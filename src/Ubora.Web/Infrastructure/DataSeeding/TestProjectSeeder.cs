using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class TestProjectSeeder
    {
        private readonly ICommandProcessor _processor;
        private readonly ApplicationUserManager _userManager;

        public TestProjectSeeder(ICommandProcessor processor, ApplicationUserManager userManager)
        {
            _processor = processor;
            _userManager = userManager;
        }

        public void SeedProject(ApplicationUser user)
        {
            CreateTestProject(user);
        }

        private void CreateTestProject(ApplicationUser user)
        {
            var userInfo = new UserInfo(user.Id, "Test User");
            var commandResult = _processor.Execute(new CreateProjectCommand
            {
                Actor = userInfo,
                NewProjectId = Guid.NewGuid(),
                Title = "Test title",
                ClinicalNeed = "Test ClinicalNeedTags",
                AreaOfUsage = "Test AreaOfUsageTags",
                PotentialTechnology = "Test PotentialTechnology",
                Gmdn = "Test Gmdn"
            });

            if (commandResult.IsFailure)
            {
                throw new Exception($"Project could not be created because: {string.Join(",", commandResult.ErrorMessages)}");
            }
        }
    }
}
