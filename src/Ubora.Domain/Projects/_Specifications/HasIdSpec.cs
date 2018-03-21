using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects._Specifications
{
    public class HasIdSpec : Specification<Project>
    {
        public Guid Id { get; }

        public HasIdSpec(Guid id)
        {
            Id = id;
        }

        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return x => x.Id == Id;
        }
    }
}
