using System;
using Marten;
using Moq;
using Ubora.Domain.Resources;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class EditResourceContentCommandHandlerTests
    {
        private readonly Mock<IDocumentSession> _documentSessionMock;
        private readonly EditResourceContentCommand.Handler _handlerUnderTest;

        public EditResourceContentCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _handlerUnderTest = new EditResourceContentCommand.Handler(_documentSessionMock.Object);
        }

        [Fact]
        public void Returns_Failed_Result_When_Content_Concurrency_Issue()
        {
            var resource = new Resource()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.ContentVersion, Guid.NewGuid());
            
            _documentSessionMock
                .Setup(x => x.Load<Resource>(resource.Id))
                .Returns(resource);

            // Act
            var commandResult = _handlerUnderTest.Handle(new EditResourceContentCommand
            {
                ResourceId = resource.Id,
                PreviousContentVersion = Guid.NewGuid()
            });

            // Assert
            commandResult.ShouldSatisfy(x => x.IsFailure);
        }
    }
}