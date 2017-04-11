using System;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Infrastructure
{
    public class CommandBusTests
    {
        [Fact]
        public void Should_Handle_Command()
        {
            var resolverMock = new Mock<IResolver>();

            var handlerMock = new Mock<ICommandHandler<TestCommand>>();

            resolverMock
                .Setup(x => x.Resolve<ICommandHandler<TestCommand>>())
                .Returns(handlerMock.Object);

            var command = new TestCommand();
            var commandResult = Mock.Of<ICommandResult>();

            handlerMock
                .Setup(x => x.Handle(command))
                .Returns(commandResult);

            var commandBus = new CommandBus(resolverMock.Object);

            // Act
            var result = commandBus.Execute(command);

            // Assert
            result.Should().BeSameAs(commandResult);
        }
    }

    public class TestCommand : ICommand
    {
    }

    public class TestCommandHandlerWithResult : ICommandHandler<TestCommand>
    {
        public ICommandResult Handle(TestCommand testCommand)
        {
            throw new NotImplementedException();
        }
    }
}
