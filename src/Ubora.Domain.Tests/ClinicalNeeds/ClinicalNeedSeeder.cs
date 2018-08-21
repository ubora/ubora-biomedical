using System;
using AutoFixture;
using Ubora.Domain.ClinicalNeeds.Commands;

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

        public ClinicalNeedSeeder IndicateTheClinicalNeed()
        {
            var command = _fixture.AutoFixture.Create<IndicateClinicalNeedCommand>();
            command.ClinicalNeedId = ClinicalNeedId;

            var commandResult = _fixture.Processor.Execute(command);
            if (commandResult.IsFailure)
            {
                throw new Exception(commandResult.ToString());
            }
            return this;
        }
    }
}