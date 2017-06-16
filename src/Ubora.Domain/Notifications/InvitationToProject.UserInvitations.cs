using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications
{
    public class UserInvitations : Specification<InvitationToProject>
    {
        protected Guid _userId;

        public UserInvitations(Guid userId)
        {
            _userId = userId;
        }

        internal override Expression<Func<InvitationToProject, bool>> ToExpression()
        {
            return x => x.InviteTo == _userId;
        }
    }
}
