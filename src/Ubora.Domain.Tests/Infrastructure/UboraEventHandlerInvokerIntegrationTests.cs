using System;
using System.Threading;
using Autofac;
using FluentAssertions;
using Marten.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects._Events;
using Xunit;

namespace Ubora.Domain.Tests.Infrastructure
{
    /// <summary>
    /// Under test: <see cref="Ubora.Domain.Infrastructure.UboraEventHandlerInvoker"/>
    /// </summary>
    public class UboraEventHandlerInvokerIntegrationTests : IntegrationFixture
    {
        private TestUboraEventHandler<ProjectCreatedEvent> _testEventHandler1;
        private TestUboraEventHandler<ProjectCreatedEvent> _testEventHandler2;
        private TestUboraEventHandler<WorkpackageOneStepEditedEvent> _testEventHandlerForOtherEventType;

        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            _testEventHandler1 = new TestUboraEventHandler<ProjectCreatedEvent>();
            _testEventHandler2 = new TestUboraEventHandler<ProjectCreatedEvent>();
            _testEventHandlerForOtherEventType = new TestUboraEventHandler<WorkpackageOneStepEditedEvent>();

            builder.RegisterInstance(_testEventHandler1).As<IUboraEventHandler<ProjectCreatedEvent>>().SingleInstance();
            builder.RegisterInstance(_testEventHandler2).As<IUboraEventHandler<ProjectCreatedEvent>>().SingleInstance();
            builder.RegisterInstance(_testEventHandlerForOtherEventType).As<IUboraEventHandler<WorkpackageOneStepEditedEvent>>().SingleInstance();
        }

        [Fact]
        public void EventHandlers_Are_Invoked_When_Event_Is_Committed()
        {
            var expectedEvent = new ProjectCreatedEvent(new DummyUserInfo(), Guid.NewGuid(), "", "", "", "", "");

            // Act (1/2)
            Session.Events.Append(stream: Guid.NewGuid(), events: expectedEvent);

            // Assert (1/2) - handlers are not invoked before commit.
            _testEventHandler1.HasHandleBeenCalled.Should().BeFalse();
            _testEventHandler2.HasHandleBeenCalled.Should().BeFalse();

            // Act (2/2)
            Session.SaveChanges();
            
            // Assert (2/2)
            _testEventHandler1.HasHandleBeenCalled.Should().BeTrue();
            _testEventHandler1.HandleArgument.Data.ShouldBeEquivalentTo(expectedEvent);

            _testEventHandler2.HasHandleBeenCalled.Should().BeTrue();
            _testEventHandler2.HandleArgument.Data.ShouldBeEquivalentTo(expectedEvent);

            _testEventHandlerForOtherEventType.HasHandleBeenCalled.Should().BeFalse();
        }
    }

    public class TestUboraEventHandler<TEvent> : IUboraEventHandler<TEvent> where TEvent : UboraEvent
    {
        public IEvent HandleArgument { get; set; }
        public bool HasHandleBeenCalled => (HandleArgument != null);

        public void Handle(IEvent eventWithMetadata)
        {
            if (HasHandleBeenCalled) { throw new Exception(); }
            HandleArgument = eventWithMetadata;
        }
    }
}
