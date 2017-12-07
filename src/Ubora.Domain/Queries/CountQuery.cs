using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Linq;
using Marten.Linq.Model;
using Marten.Services.Includes;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Queries
{
    public class CountQuery<T> : IQuery<int>
    {
        public CountQuery(ISpecification<T> specification)
        {
            Specification = specification;
        }
        public ISpecification<T> Specification { get; private set; }

        internal class Handler : IQueryHandler<CountQuery<T>, int> 
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public int Handle(CountQuery<T> query)
            {
                return query.Specification.SatisfyEntitiesFrom(_querySession.Query<T>()).Count();
            }
        }
    }
}
