using System;
using System.Collections.Generic;
using System.Text;
using Marten;

namespace Ubora.Domain.Queries
{
    public interface IQuery
    {
        IEnumerable<T> Find<T>();
        T Load<T>(Guid id);
    }

    public class Query : IQuery
    {
        private readonly IQuerySession _querySession;

        public Query(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        public IEnumerable<T> Find<T>()
        {
            return _querySession.Query<T>();
        }

        public T Load<T>(Guid id)
        {
            return _querySession.Load<T>(id);
        }
    }
}
