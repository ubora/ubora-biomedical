using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ubora.Domain.Infrastructure.Specifications
{
    public class MatchAll<T> : Specification<T>
    {
        internal override Expression<Func<T, bool>> ToExpression()
        {
            return x => true;
        }
    }
}
