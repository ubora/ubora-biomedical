using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Marten;
using Marten.Events;
using Moq;
using Ubora.Domain.Projects.IsoStandardsCompliances;
using Ubora.Domain.Projects.IsoStandardsCompliances.Commands;
using Ubora.Domain.Projects.IsoStandardsCompliances.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.IsoStandardsCompliances.Commands
{
    public class RemoveIsoStandardCommandHandlerTests
    {
        private IFixture AutoFixture { get; } = new Fixture();
        private readonly Mock<IDocumentSession> _documentSessionMock;

        public RemoveIsoStandardCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);
        }

        [Fact]
        public void HappyPath_Persists_Event()
        {
            var handlerUnderTest = new RemoveIsoStandardCommand.Handler(_documentSessionMock.Object);

            var command = AutoFixture.Create<RemoveIsoStandardCommand>();

            _documentSessionMock
                .Setup(x => x.Load<IsoStandardsComplianceAggregate>(command.ProjectId))
                .Returns(new IsoStandardsComplianceAggregate().Set(x => x.Id, command.ProjectId));

            object[] appendedEvents = null;
            _documentSessionMock
                .Setup(x => x.Events.Append(command.ProjectId, It.IsAny<object[]>()))
                .Callback<Guid, object[]>((_, e) => appendedEvents = e)
                .Returns(new EventStream(Guid.Empty, false));

            _documentSessionMock.Setup(x => x.SaveChanges());

            // Act
            var result = handlerUnderTest.Handle(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var @event = (IsoStandardRemovedFromComplianceChecklistEvent)appendedEvents.Single();

            @event.AggregateId.Should().Be(command.ProjectId);
            @event.ProjectId.Should().Be(command.ProjectId);
            @event.InitiatedBy.Should().Be(command.Actor);

            _documentSessionMock.VerifyAll();
        }
    }
}