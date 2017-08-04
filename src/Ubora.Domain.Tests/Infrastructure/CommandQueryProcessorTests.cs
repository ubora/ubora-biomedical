using System;
using Autofac;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Queries;
using Xunit;

namespace Ubora.Domain.Tests.Infrastructure
{
    public class CommandQueryProcessorTests : IntegrationFixture
    {
        [Fact]
        public void Should_Handle_Command()
        {
            var command = new TestCommand();
            var commandResult = Mock.Of<ICommandResult>();
            var commandHandler = Mock.Of<ICommandHandler<TestCommand>>(x => x.Handle(command) == commandResult);

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(commandHandler);
            var container = containerBuilder.Build();

            var processor = new CommandQueryProcessor(container, Mock.Of<IQuerySession>());

            // Act
            var result = processor.Execute(command);

            // Assert
            result.Should().BeSameAs(commandResult);
        }

        [Fact]
        public void Should_Handle_Query()
        {
            var query = new TestQuery();
            var queryResult = Guid.NewGuid();
            var queryHandler = Mock.Of<IQueryHandler<TestQuery, Guid>>(x => x.Handle(query) == queryResult);

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(queryHandler);
            var container = containerBuilder.Build();

            var processor = new CommandQueryProcessor(container, Mock.Of<IQuerySession>());

            // Act
            var result = processor.ExecuteQuery(query);

            // Assert
            result.Should().Be(queryResult);
        }
    }

    public class TestCommand : ICommand
    {
    }

    public class TestQuery : IQuery<Guid>
    {
    }
}
