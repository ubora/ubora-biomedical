using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects._Specifications
{
    public class IsFromProjectSpec<TEntity> : Specification<TEntity> where TEntity : IProjectEntity
    {
        public Guid ProjectId { get; set; }

        internal override Expression<Func<TEntity, bool>> ToExpression()
        {
            return e => e.ProjectId == ProjectId;
        }
    }
}
