using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Xunit;

namespace Ubora.Domain.Tests.Infrastructure
{
    public class ProjectionTests
    {
        [Fact]
        public void Apply_Should_Project_IQueryable_With_Select_Expression()
        {
            var input = new[] {new Doc1 {Val = "one"}, new Doc1 { Val = "two" } }.AsQueryable();
            // Act
            var output = new TestProjection().Apply(input);
            // Assert
            output.Select(o => o.Val).Should().ContainInOrder(input.Select(i => i.Val));
        }

        [Fact]
        public void Apply_Should_Project_IEnumerable_With_Select_Expression()
        {
            var input = new[] { new Doc1 { Val = "one" }, new Doc1 { Val = "two" } };
            // Act
            var output = new TestProjection().Apply(input);
            // Assert
            output.Select(o => o.Val).Should().ContainInOrder(input.Select(i => i.Val));
        }


        public class TestProjection : Projection<Doc1, Doc2>
        {
            protected override Expression<Func<Doc1, Doc2>> SelectExpression => s => new Doc2 { Val = s.Val };
        }

        public class Doc1
        {
            public string Val { get; set; }
        }
        public class Doc2
        {
            public string Val { get; set; }
        }

    }
}
