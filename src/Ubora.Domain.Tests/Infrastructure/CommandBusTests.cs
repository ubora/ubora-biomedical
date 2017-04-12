using Autofac;
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
            var containerBuilder = new ContainerBuilder();

            var command = new TestCommand();
            var commandResult = Mock.Of<ICommandResult>();
            var commandHandler = Mock.Of<ICommandHandler<TestCommand>>(x => x.Handle(command) == commandResult);

            containerBuilder.RegisterInstance(commandHandler);

            var container = containerBuilder.Build();

            var commandBus = new CommandQueryBus(container);

            // Act
            var result = commandBus.Execute(command);

            // Assert
            result.Should().BeSameAs(commandResult);
        }
    }

    public class TestCommand : ICommand
    {
    }
}
