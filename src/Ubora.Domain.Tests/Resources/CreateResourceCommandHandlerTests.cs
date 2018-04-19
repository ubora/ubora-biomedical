using System;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.Resources;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class CreateResourceCommandHandlerTests
    {
        private readonly CreateResourceCommand.Handler _handlerUnderTest;
        private readonly Mock<IDocumentSession> _documentSessionMock;

        public CreateResourceCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _handlerUnderTest = new CreateResourceCommand.Handler(_documentSessionMock.Object);
        }

        [Fact]
        public void Throws_When_Page_With_ID_Already_Exists()
        {
            var resourceId = Guid.NewGuid();
            _documentSessionMock
                .Setup(x => x.Load<Resource>(resourceId))
                .Returns(Mock.Of<Resource>());
            
            // Act
            Action act = () => _handlerUnderTest
                .Handle(new CreateResourceCommand 
                {
                    ResourceId = resourceId
                });
            
            // Assert
            act.ShouldThrow<InvalidOperationException>()
                .Where(ex => ex.Message.Contains("ID"));
        }
    }
}