using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Marten;
using Marten.Events;
using Moq;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Commands;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.IsoStandardsComplianceChecklists.Commands
{
    public class MarkIsoStandardAsNoncompliantCommandHandlerTests
    {
        private IFixture AutoFixture { get; } = new Fixture();
        private readonly Mock<IDocumentSession> _documentSessionMock;

        public MarkIsoStandardAsNoncompliantCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);
        }

        [Fact]
        public void HappyPath_Persists_Event()
        {
            var handlerUnderTest = new MarkIsoStandardAsNoncompliantCommand.Handler(_documentSessionMock.Object);

            var command = AutoFixture.Create<MarkIsoStandardAsNoncompliantCommand>();

            _documentSessionMock
                .Setup(x => x.Load<IsoStandardsComplianceChecklist>(command.ProjectId))
                .Returns(new IsoStandardsComplianceChecklist().Set(x => x.Id, command.ProjectId));

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

            var @event = (IsoStandardMarkedAsNoncompliantEvent)appendedEvents.Single();

            @event.AggregateId.Should().Be(command.ProjectId);
            @event.ProjectId.Should().Be(command.ProjectId);
            @event.InitiatedBy.Should().Be(command.Actor);

            _documentSessionMock.VerifyAll();
        }
    }
}