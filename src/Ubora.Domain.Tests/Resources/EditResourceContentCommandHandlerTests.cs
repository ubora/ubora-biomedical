﻿using System;
using Marten;
using Moq;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class EditResourceContentCommandHandlerTests
    {
        private readonly Mock<IDocumentSession> _documentSessionMock;
        private readonly EditResourcePageContentCommand.Handler _handlerUnderTest;

        public EditResourceContentCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _handlerUnderTest = new EditResourcePageContentCommand.Handler(_documentSessionMock.Object);
        }

        [Fact]
        public void Returns_Failed_Result_When_Content_Concurrency_Issue()
        {
            var resource = new ResourcePage()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.BodyVersion, 1);
            
            _documentSessionMock
                .Setup(x => x.Load<ResourcePage>(resource.Id))
                .Returns(resource);

            // Act
            var commandResult = _handlerUnderTest.Handle(new EditResourcePageContentCommand
            {
                ResourceId = resource.Id,
                PreviousContentVersion = 2,
                Actor = new DummyUserInfo()
            });

            // Assert
            commandResult.ShouldSatisfy(x => x.IsFailure);
        }
    }
}