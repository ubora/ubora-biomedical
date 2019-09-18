using System;
using Ubora.Domain.ClinicalNeeds.Commands;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Web.Data;

namespace Ubora.Web.Infrastructure.DataSeeding
{
    public class TestClinicalNeedSeeder
    {
        private readonly ICommandProcessor _processor;

        public TestClinicalNeedSeeder(ICommandProcessor processor)
        {
            _processor = processor;
        }

        public void SeedClinicalNeed(ApplicationUser user)
        {
            var userInfo = new UserInfo(user.Id, "Test User");

            var commandResult = _processor.Execute(new IndicateClinicalNeedCommand
            {
                Actor = userInfo,
                ClinicalNeedId = Guid.NewGuid(),
                Title = "Clinical need title",
                Description = new Domain.QuillDelta(),
                ClinicalNeedTag = "Test tag",
                AreaOfUsageTag = "Some area",
                PotentialTechnologyTag = "Some technology",
                Keywords = "keyword0 keyword1"
            });

            if (commandResult.IsFailure)
            {
                throw new Exception($"Clinical need could not be created because: {string.Join(",", commandResult.ErrorMessages)}");
            }
        }
    }
}
