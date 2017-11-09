using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Data;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class TestProjectSeeder
    {
        private readonly ICommandProcessor _processor;

        public TestProjectSeeder(ICommandProcessor processor)
        {
            _processor = processor;
        }

        public void SeedProject(ApplicationUser user)
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
