using System;
using AutoFixture;
using Ubora.Domain.ClinicalNeeds.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Tests.ClinicalNeeds
{
    public class ClinicalNeedSeeder
    {
        private readonly IntegrationFixture _fixture;

        public ClinicalNeedSeeder(IntegrationFixture fixture, Guid? clinicalNeedId = null)
        {
            _fixture = fixture;
            ClinicalNeedId = clinicalNeedId ?? Guid.NewGuid();
        }

        public Guid ClinicalNeedId { get; set; }

        public ClinicalNeedSeeder IndicateTheClinicalNeed(Guid? indicatorId = null)
        {
            if (indicatorId == null)
            {
                indicatorId = Guid.NewGuid();
                _fixture.Create_User(indicatorId.Value);
            }

            var command = _fixture.AutoFixture.Create<IndicateClinicalNeedCommand>();
            command.ClinicalNeedId = ClinicalNeedId;
            command.Actor = new UserInfo(indicatorId.Value, "dummy");

            var commandResult = _fixture.Processor.Execute(command);
            if (commandResult.IsFailure)
            {
                throw new Exception(commandResult.ToString());
            }
            return this;
        }

        public ClinicalNeedSeeder EditTheClinicalNeed(
            string title = null, 
            QuillDelta description = null, 
            string clinicalNeedTag = "", 
            string areaOfUsageTag = "", 
            string potentialTechnologyTag = "", 
            string keywords = "")
        {
            var command = new EditClinicalNeedCommand
            {
                ClinicalNeedId = ClinicalNeedId,
                Title = title ?? Guid.NewGuid().ToString(),
                Description = description ?? new QuillDelta(),
                ClinicalNeedTag = clinicalNeedTag ?? Guid.NewGuid().ToString(),
                AreaOfUsageTag = areaOfUsageTag ?? Guid.NewGuid().ToString(),
                PotentialTechnologyTag = potentialTechnologyTag ?? Guid.NewGuid().ToString(),
                Keywords = keywords ?? Guid.NewGuid().ToString(),
                Actor = new DummyUserInfo()
            };

            var commandResult = _fixture.Processor.Execute(command);
            if (commandResult.IsFailure)
            {
                throw new Exception(commandResult.ToString());
            }
            return this;
        }
    }
}