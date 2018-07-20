using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Marten;
using Ubora.Domain.Projects.IsoStandardsCompliances;
using Ubora.Domain.Projects.IsoStandardsCompliances.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.IsoStandardsCompliances
{
    public class IsoStandardsComplianceAggregateIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Apply_IsoStandardAddedToComplianceChecklistEvent_HappyPath()
        {
            var projectId = Guid.NewGuid();
            var aggregateId = Guid.NewGuid();
            var actor = new DummyUserInfo();
            var @event = new IsoStandardAddedToComplianceChecklistEvent(actor, projectId, aggregateId, "testTitle", "testDescription", new Uri("https://www.google.com"));

            // Act
            var aggregate = Session.EventsAppendAndSaveAndLoad<IsoStandardsComplianceAggregate>(aggregateId, @event);

            // Assert
            var standard = aggregate.IsoStandards.Single();

            using (new AssertionScope())
            {
                aggregate.ProjectId.Should().Be(projectId);

                standard.Id.Should().NotBe(Guid.Empty);
                standard.Title.Should().Be("testTitle");
                standard.Link.Should().Be(new Uri("https://www.google.com"));
                standard.ShortDescription.Should().Be("testDescription");
                standard.IsMarkedAsCompliant.Should().BeFalse();
                standard.AddedByUserId.Should().Be(actor.UserId);
            }
        }

        [Fact]
        public void Apply_IsoStandardRemovedFromComplianceChecklistEvent_HappyPath()
        {
            var projectId = Guid.NewGuid();
            var aggregateId = Guid.NewGuid();

            Session.Events.Append(aggregateId, new IsoStandardAddedToComplianceChecklistEvent(new DummyUserInfo(), projectId, aggregateId, "testTitle", "testDescription", new Uri("https://www.google.com")));
            Session.Events.Append(aggregateId, new IsoStandardAddedToComplianceChecklistEvent(new DummyUserInfo(), projectId, aggregateId, "testTitle", "testDescription", new Uri("https://www.google.com")));
            Session.Events.Append(aggregateId, new IsoStandardAddedToComplianceChecklistEvent(new DummyUserInfo(), projectId, aggregateId, "testTitle", "testDescription", new Uri("https://www.google.com")));
            Session.SaveChanges();

            var aggregate = Session.Load<IsoStandardsComplianceAggregate>(aggregateId);
            var isoStandardToRemove = aggregate.IsoStandards.Skip(1).First();

            var @event = new IsoStandardRemovedFromComplianceChecklistEvent(new DummyUserInfo(), projectId, aggregateId, isoStandardToRemove.Id);

            // Act
            aggregate = Session.EventsAppendAndSaveAndLoad<IsoStandardsComplianceAggregate>(aggregateId, @event);

            // Assert
            using (new AssertionScope())
            {
                aggregate.IsoStandards.Should()
                    .HaveCount(2)
                    .And.NotContain(x => x.Id == isoStandardToRemove.Id);
            }
        }

        [Fact]
        public void Apply_IsoStandardMarkedAsCompliantEvent_HappyPath()
        {
            var projectId = Guid.NewGuid();
            var aggregateId = Guid.NewGuid();

            var aggregate = Session.EventsAppendAndSaveAndLoad<IsoStandardsComplianceAggregate>(aggregateId, new IsoStandardAddedToComplianceChecklistEvent(new DummyUserInfo(), projectId, aggregateId, "testTitle", "testDescription", new Uri("https://www.google.com")));
            var isoStandardBeforeMarking = aggregate.IsoStandards.Single();

            var @event = new IsoStandardMarkedAsCompliantEvent(new DummyUserInfo(), projectId, aggregateId, isoStandardBeforeMarking.Id);

            // Act
            aggregate = Session.EventsAppendAndSaveAndLoad<IsoStandardsComplianceAggregate>(aggregateId, @event);

            // Assert
            var standardAfterMarking = aggregate.IsoStandards.Single();

            using (new AssertionScope())
            {
                standardAfterMarking.ShouldBeEquivalentTo(isoStandardBeforeMarking, opt => opt.Excluding(x => x.IsMarkedAsCompliant));
                standardAfterMarking.IsMarkedAsCompliant.Should().BeTrue();
            }
        }

        [Fact]
        public void Apply_IsoStandardMarkedAsNoncompliantEvent_HappyPath()
        {
            var projectId = Guid.NewGuid();
            var aggregateId = Guid.NewGuid();

            var aggregate = Session.EventsAppendAndSaveAndLoad<IsoStandardsComplianceAggregate>(
                aggregateId, 
                new IsoStandardAddedToComplianceChecklistEvent(new DummyUserInfo(), projectId, aggregateId, "testTitle", "testDescription", new Uri("https://www.google.com")));

            var isoStandardId = aggregate.IsoStandards.Single().Id;

            aggregate = Session.EventsAppendAndSaveAndLoad<IsoStandardsComplianceAggregate>(
                aggregateId, 
                new IsoStandardMarkedAsCompliantEvent(new DummyUserInfo(), projectId, aggregateId, 
                    isoStandardId: isoStandardId));

            var isoStandardBeforeMarking = aggregate.IsoStandards.Single();

            var @event = new IsoStandardMarkedAsNoncompliantEvent(new DummyUserInfo(), projectId, aggregateId, isoStandardId);

            // Act
            aggregate = Session.EventsAppendAndSaveAndLoad<IsoStandardsComplianceAggregate>(aggregateId, @event);

            // Assert
            var standardAfterMarking = aggregate.IsoStandards.Single();

            using (new AssertionScope())
            {
                standardAfterMarking.ShouldBeEquivalentTo(isoStandardBeforeMarking, opt => opt.Excluding(x => x.IsMarkedAsCompliant));
                standardAfterMarking.IsMarkedAsCompliant.Should().BeFalse();
            }
        }
    }
}
