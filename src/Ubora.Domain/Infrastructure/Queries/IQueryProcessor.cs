using System;
using System.Collections.Generic;
using Marten.Pagination;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure.Queries
{
    public interface IQueryProcessor
    {
        T ExecuteQuery<T>(IQuery<T> query);

        IPagedList<T> Find<T>(
            ISpecification<T> specification);

        IPagedList<T> Find<T>(
            ISpecification<T> specification,
            int pageSize, 
            int pageNumber);

        IPagedList<TProjection> Find<T, TProjection>(
            ISpecification<T> specification,
            IProjection<T, TProjection> projection,
            int pageSize = int.MaxValue, 
            int pageNumber = 1);
        
        IPagedList<T> Find<T>(
            ISpecification<T> specification, 
            ISortSpecification<T> sortSpecification,
            int pageSize, 
            int pageNumber);

        IPagedList<TProjection> Find<T, TProjection>(
            ISpecification<T> specification,
            IProjection<T, TProjection> projection,
            ISortSpecification<TProjection> sortSpecification, 
            int pageSize, 
            int pageNumber);

        T FindById<T>(Guid id); // Replace calls with specification?
    }
}