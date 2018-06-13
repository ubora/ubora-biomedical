using System;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class CreateResourcePageCommandHandlerTests
    {
        private readonly CreateResourcePageCommand.Handler _handlerUnderTest;
        private readonly Mock<IDocumentSession> _documentSessionMock;

        public CreateResourcePageCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _handlerUnderTest = new CreateResourcePageCommand.Handler(_documentSessionMock.Object);
        }

        [Fact]
        public void Throws_When_Page_With_ID_Already_Exists()
        {
            var resourceId = Guid.NewGuid();
            _documentSessionMock
                .Setup(x => x.Load<ResourcePage>(resourceId))
                .Returns(Mock.Of<ResourcePage>());
            
            // Act
            Action act = () => _handlerUnderTest
                .Handle(new CreateResourcePageCommand 
                {
                    ResourceId = resourceId
                });
            
            // Assert
            act.ShouldThrow<InvalidOperationException>()
                .Where(ex => ex.Message.Contains("ID"));
        }
    }
}