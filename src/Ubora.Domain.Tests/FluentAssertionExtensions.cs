using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Ubora.Domain.Tests
{
    public static class FluentAssertionExtensions
    {
        public static AndConstraint<ObjectAssertions> ShouldSatisfy<T>(this T @this, Expression<Func<T, bool>> predicate)
        {
            return @this.Should().Match<T>(predicate);
        }
    }
}