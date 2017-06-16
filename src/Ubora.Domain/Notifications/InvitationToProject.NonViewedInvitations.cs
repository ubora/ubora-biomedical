using System;
using System.Linq.Expressions;

namespace Ubora.Domain.Notifications
{
    public class NonViewedInvitations : UserInvitations
    {
        public NonViewedInvitations(Guid userId):base(userId)
        {
        }

        internal override Expression<Func<InvitationToProject, bool>> ToExpression()
        {
            return x => x.InviteTo == _userId && !x.HasBeenViewed;
        }
    }
}
