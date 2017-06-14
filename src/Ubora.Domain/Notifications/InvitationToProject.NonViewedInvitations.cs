using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class NonViewedInvitations : Specification<InvitationToProject>
    {
        private Guid _userId;

        public NonViewedInvitations(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<InvitationToProject, bool>> ToExpression()
        {
            return x => x.InvitedMemberId == _userId && !x.HasBeenViewed;
        }
    }
}
