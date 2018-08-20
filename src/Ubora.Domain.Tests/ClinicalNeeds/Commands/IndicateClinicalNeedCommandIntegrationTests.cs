using System.Linq;
using Xunit;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.ClinicalNeeds.Commands;
using Ubora.Domain.ClinicalNeeds.Events;
using Ubora.Domain.Discussions;
using Ubora.Domain.Discussions.Events;

namespace Ubora.Domain.Tests.ClinicalNeeds.Commands
{
    public class IndicateClinicalNeedCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Clinical_Need_Can_Be_Indicated()
        {
            var command = AutoFixture.Create<IndicateClinicalNeedCommand>();

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var event1 = Session.Events.QueryRawEventDataOnly<ClinicalNeedIndicatedEvent>().Single();
            event1.InitiatedBy.Should().Be(command.Actor);

            var clinicalNeed = Session.Load<ClinicalNeed>(command.ClinicalNeedId);
            clinicalNeed.Should().NotBeNull();

            using (new AssertionScope())
            {
                clinicalNeed.Id.Should().Be(command.ClinicalNeedId);
                clinicalNeed.DiscussionId.Should().Be(command.ClinicalNeedId);
                clinicalNeed.Title.Should().Be(command.Title);
                clinicalNeed.Description.Should().Be(command.Description);
                clinicalNeed.ClinicalNeedTag.Should().Be(command.ClinicalNeedTag);
                clinicalNeed.AreaOfUsageTag.Should().Be(command.AreaOfUsageTag);
                clinicalNeed.PotentialTechnologyTag.Should().Be(command.PotentialTechnologyTag);
                clinicalNeed.Keywords.Should().Be(command.Keywords);
                clinicalNeed.IndicatedAt.Should().Be(event1.Timestamp);
                clinicalNeed.IndicatorUserId.Should().Be(command.Actor.UserId);
            }

            var event2 = Session.Events.QueryRawEventDataOnly<DiscussionOpenedEvent>().Single();
            event2.InitiatedBy.Should().Be(command.Actor);

            var discussion = Session.Load<Discussion>(command.ClinicalNeedId);
            discussion.Should().NotBeNull();

            using (new AssertionScope())
            {
                discussion.Id.Should().Be(command.ClinicalNeedId);
                discussion.AttachedToEntity.Should().Be(new AttachedToEntity(EntityName.ClinicalNeed, command.ClinicalNeedId));
            }
        }
    }
}