using System;
using System.Linq;

namespace Ubora.Domain.Infrastructure
{
    /// <summary>
    /// NB! Only for sorting :)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISortSpecification<TEntity>
    {
        IQueryable<TEntity> Sort(IQueryable<TEntity> query);
    }
}