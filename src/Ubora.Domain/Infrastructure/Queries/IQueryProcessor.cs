using System;
using System.Collections.Generic;
using Marten.Pagination;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure.Queries
{
    public interface IQueryProcessor
    {
        T ExecuteQuery<T>(IQuery<T> query);

        IPagedList<T> Find<T>(ISpecification<T> specification);

        IPagedList<T> Find<T>(ISpecification<T> specification,
            int pageSize, int pageNumber);

        IPagedList<T> Find<T>(ISpecification<T> specification, ISortSpecification<T> sortSpecification,
            int pageSize, int pageNumber);

        IPagedList<TDocumentProjection> Find<TDocument, TDocumentProjection>(
            ISpecification<TDocument> specification,
            IProjection<TDocument, TDocumentProjection> projection,
            ISortSpecification<TDocumentProjection> sortSpecification, int pageSize, int pageNumber);
        T FindById<T>(Guid id); // Replace calls with specification?
    }
}