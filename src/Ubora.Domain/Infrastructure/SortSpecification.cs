using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ubora.Domain.Infrastructure
{
    /// <summary>
    /// Do not use this interface directly for implementing specifications, use abstract SortSpecification<TEntity, Tkey> class for that.
    /// </summary>
    public interface ISortSpecification<TEntity>
    {
        IQueryable<TEntity> Sort(IQueryable<TEntity> query);
    }

    public abstract class SortSpecification<TEntity, TKey> : ISortSpecification<TEntity>
    {
        public IQueryable<TEntity> Sort(IQueryable<TEntity> query)
        {
            return Queryable.OrderBy(query, KeySelector);
        }

        internal abstract Expression<Func<TEntity, TKey>> KeySelector { get; }
    }
}