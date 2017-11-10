using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Marten;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Projects.History;
using Ubora.Domain.Projects._Events;
using Xunit;

namespace Ubora.Domain.Tests
{
    public class EventToHistoryTransformerIntegrationFixture : DocumentSessionIntegrationFixture
    {
        public EventToHistoryTransformerIntegrationFixture()
        {
            StoreOptions(o =>
            {
                new UboraStoreOptionsConfigurer().CreateConfigureAction(new List<Type> {typeof(TestEvent) }, new List<MappedType>(),
                    AutoCreate.All).Invoke(o);
            });
        }

        [Fact]
        public void Must_Transform_Event_To_History_EventLogEntry()
        {
            var testEvent = new TestEvent(new UserInfo(Guid.NewGuid(), "test user"), Guid.NewGuid());
            
            // Act
            Session.Events.Append(testEvent.ProjectId, testEvent);
            Session.SaveChanges();
            
            // Assert
            RefreshSession();
            var logEntry = Session.Query<EventLogEntry>().Single();
            logEntry.Event.ShouldBeEquivalentTo(testEvent);
            logEntry.ProjectId.Should().Be(testEvent.ProjectId);
            logEntry.Event.GetDescription().Should().Be(testEvent.GetDescription());
            logEntry.Timestamp.Should().Be(testEvent.Timestamp);
            logEntry.UserId.Should().Be(testEvent.InitiatedBy.UserId);
        }
        
        

        public class TestEvent : ProjectEvent
        {
            public TestEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy, projectId)
            {
            }

            public override string GetDescription()
            {
                return nameof(TestEvent) + "-" + ProjectId;
            }
        }

    }
}