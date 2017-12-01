using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Marten;
using Marten.Linq;
using Moq;
using TestStack.BDDfy.Annotations;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Infrastructure
{
    public class CommandQueryProcessorTests : IntegrationFixture
    {
        [Fact]
        public void Execute_Should_Handle_Command()
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
        public void ExecuteQuery_Should_Handle_Query()
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

        [Fact]
        public void Find_Should_Return_Results_Filtered_By_Specification_And_Ordered_By_Specification()
        {
            var querySessionMock = new Mock<IQuerySession>();
            var query = Mock.Of<IMartenQueryable<object>>();
            querySessionMock.Setup(q => q.Query<object>())
                .Returns(query);
            var specificationMock = new Mock<ISpecification<object>>();
            var satisfiedBySpecification = new[] { new object(), new object() }.AsQueryable();
            specificationMock.Setup(s => s.SatisfyEntitiesFrom(query))
                .Returns(satisfiedBySpecification);
            var sortSpecMock = new Mock<ISortSpecification<object>>();
            var sortedBySpec = new[] { new object(), new object(), new object(), new object(), new object(), new object() }.AsQueryable();
            sortSpecMock.Setup(s => s.Sort(satisfiedBySpecification))
                .Returns(sortedBySpec);
            var processor = new CommandQueryProcessor(new ContainerBuilder().Build(), querySessionMock.Object);

            // Act
            var result = processor.Find(specificationMock.Object, sortSpecMock.Object, 3, 2);

            // Assert
            result.Should().BeEquivalentTo(sortedBySpec.Skip(3).Take(3));
        }

        [Fact]
        public void Find_Overload_With_Projection_Argument_Should_Apply_Projection()
        {
            var querySessionMock = new Mock<IQuerySession>();
            var query = Mock.Of<IMartenQueryable<object>>();
            querySessionMock.Setup(q => q.Query<object>())
                .Returns(query);
            var specificationMock = new Mock<ISpecification<object>>();
            var satisfiedBySpecification = new[] { new object(), new object() }.AsQueryable();
            specificationMock.Setup(s => s.SatisfyEntitiesFrom(query))
                .Returns(satisfiedBySpecification);
            var processor = new CommandQueryProcessor(new ContainerBuilder().Build(), querySessionMock.Object);
            var projectionApplied = new[] { new object(), new object() }.AsQueryable();
            var projectionMock = new Mock<IProjection<object, object>>();
            projectionMock.Setup(p => p.Apply(satisfiedBySpecification))
                .Returns(projectionApplied);
            var sortSpecMock = new Mock<ISortSpecification<object>>();
            var sortedBySpec = new[] { new object(), new object(), new object(), new object(), new object(), new object() }.AsQueryable();
            sortSpecMock.Setup(s => s.Sort(projectionApplied))
                .Returns(sortedBySpec);

            // Act
            var result = processor.Find(specificationMock.Object, projectionMock.Object, sortSpecMock.Object, 3, 2);

            // Assert
            result.Should().BeEquivalentTo(sortedBySpec.Skip(3).Take(3));
        }

        [Fact]
        public void Find_Overload_With_Only_Specification_Argument_Is_Delegated_Correctly()
        {
            var spec = Mock.Of<ISpecification<object>>();
            var expectedResult = new PagedListStub<object>();
            var commandQueryProcessorMock = new Mock<CommandQueryProcessor>(Mock.Of<IComponentContext>(), Mock.Of<IQuerySession>()) {CallBase = true};
            commandQueryProcessorMock.Setup(c => c.Find(spec, null, int.MaxValue, 1))
                .Returns(expectedResult);
           
            // Act
            var result = commandQueryProcessorMock.Object.Find(spec);
            // Assert
            result.Should().BeSameAs(expectedResult);
        }

        [Fact]
        public void Find_Overload_With_Sepcification_And_Page_Arguments_Is_Delegated_Correctly()
        {
            var spec = Mock.Of<ISpecification<object>>();
            var expectedResult = new PagedListStub<object>();
            var commandQueryProcessorMock = new Mock<CommandQueryProcessor>(Mock.Of<IComponentContext>(), Mock.Of<IQuerySession>()) { CallBase = true };
            commandQueryProcessorMock.Setup(c => c.Find(spec, null, 5, 2))
                .Returns(expectedResult);

            // Act
            var result = commandQueryProcessorMock.Object.Find(spec, 5, 2);
            // Assert
            result.Should().BeSameAs(expectedResult);
        }

        public class TestCommand : ICommand
        {
        }

        public class TestQuery : IQuery<Guid>
        {
        }
    }
}
