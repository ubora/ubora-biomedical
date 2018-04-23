using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects._Projections
{
    public class IdProjection : Projection<Project, Guid>
    {
        protected override Expression<Func<Project, Guid>> ToSelector()
        {
            return project => project.Id;
        }
    }
}
