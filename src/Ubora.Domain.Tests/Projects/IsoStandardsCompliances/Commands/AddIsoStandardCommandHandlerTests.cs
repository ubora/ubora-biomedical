using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Marten;
using Marten.Events;
using Moq;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.IsoStandardsCompliances.Commands;
using Ubora.Domain.Projects.IsoStandardsCompliances.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.IsoStandardsCompliances.Commands
{
    public class AddIsoStandardCommandHandlerTests
    {
        private IFixture AutoFixture { get; } = new Fixture();
        private readonly Mock<IDocumentSession> _documentSessionMock;

        public AddIsoStandardCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);
        }

        [Fact]
        public void HappyPath_Persists_Event()
        {
            var handlerUnderTest = new AddIsoStandardCommand.Handler(_documentSessionMock.Object);

            var command = AutoFixture.Create<AddIsoStandardCommand>();

            _documentSessionMock
                .Setup(x => x.Load<Project>(command.ProjectId))
                .Returns(new Project().Set(x => x.Id, command.ProjectId));

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

            var @event = (IsoStandardAddedToComplianceChecklistEvent)appendedEvents.Single();

            @event.AggregateId.Should().Be(command.ProjectId);
            @event.ProjectId.Should().Be(command.ProjectId);
            @event.Link.Should().Be(command.Link);
            @event.Title.Should().Be(command.Title);
            @event.ShortDescription.Should().Be(command.ShortDescription);
            @event.InitiatedBy.Should().Be(command.Actor);

            _documentSessionMock.VerifyAll();
        }
    }
}
