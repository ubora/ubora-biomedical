using System;
using System.Linq.Expressions;
using FluentAssertions.Primitives;

// ReSharper disable once CheckNamespace
namespace FluentAssertions
{
    public static class FluentAssertionExtensions
    {
        public static AndConstraint<ObjectAssertions> ShouldSatisfy<T>(this T @this, Expression<Func<T, bool>> predicate)
        {
            return @this.Should().Match<T>(predicate);
        }
    }
}