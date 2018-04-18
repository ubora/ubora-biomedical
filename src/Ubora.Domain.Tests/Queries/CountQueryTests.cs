using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Marten;
using Marten.Linq;
using Moq;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Queries;
using Xunit;

namespace Ubora.Domain.Tests.Queries
{
    public class CountQueryTests
    {
        [Fact]
        public void Should_Return_Count_Of_Satisfied_Items()
        {
            var specificationMock = new Mock<ISpecification<object>>();
            var query = new CountQuery<object>(specificationMock.Object);

            var querySessionMock = new Mock<IQuerySession>();
            var queryResult = Mock.Of<IMartenQueryable<object>>();
            querySessionMock.Setup(q => q.Query<object>())
                .Returns(queryResult);
            specificationMock.Setup(s => s.SatisfyEntitiesFrom(queryResult))
                .Returns(new[] {new object(), new object(), new object()}.AsQueryable);
            var handler = new CountQuery<object>.Handler(querySessionMock.Object);
            // Act
            var count = handler.Handle(query);

            // Assert
            count.Should().Be(3);
        }
    }
}
