using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ubora.Domain.Infrastructure
{
    public interface IProjection<TDocument, TDocumentProjection>
    {
        IQueryable<TDocumentProjection> Apply(IQueryable<TDocument> query);
    }

    public abstract class Projection<TDocument, TDocumentProjection> : IProjection<TDocument, TDocumentProjection>
    {
        public IQueryable<TDocumentProjection> Apply(IQueryable<TDocument> query)
        {
            return query.Select(SelectExpression);
        }

        protected abstract Expression<Func<TDocument, TDocumentProjection>> SelectExpression { get; }
    }
}