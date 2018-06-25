using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Candidates.Specifications
{
    public class HasCreatedByUserId : Specification<Candidate>
    {
        public Guid CreatedByUserId { get; }

        public HasCreatedByUserId(Guid createdByUserId)
        {
            CreatedByUserId = createdByUserId;
        }

        internal override Expression<Func<Candidate, bool>> ToExpression()
        {
            return x => x.CreatedByUserId == CreatedByUserId;
        }
    }
}
