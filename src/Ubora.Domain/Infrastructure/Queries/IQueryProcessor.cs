using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure.Queries
{
    public interface IQueryProcessor
    {
        T ExecuteQuery<T>(IQuery<T> query);
        IEnumerable<T> Find<T>(ISpecification<T> specification = null);
        T FindById<T>(Guid id); // Replace calls with specification?
    }
}