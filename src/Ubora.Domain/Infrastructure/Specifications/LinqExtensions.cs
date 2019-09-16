using System.Collections.Generic;
using Ubora.Domain.Infrastructure;
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

        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, ISortSpecification<T> specification)
        {
            return specification.Sort(queryable);
        }

        public static IEnumerable<T> Sort<T>(this IEnumerable<T> enumerable, ISortSpecification<T> specification)
        {
            return specification.Sort(enumerable.AsQueryable());
        }
    }
}
