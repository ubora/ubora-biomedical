using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Specifications;

// ReSharper disable once CheckNamespace
namespace System.Linq
{
    public static class SpecificationLinqExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> queryable, ISpecification<T> specification)
        {
            return specification.SatisfyEntitiesFrom(queryable);
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, ISpecification<T> specification)
        {
            return specification.SatisfyEntitiesFrom(enumerable.AsQueryable());
        }
    }
}
