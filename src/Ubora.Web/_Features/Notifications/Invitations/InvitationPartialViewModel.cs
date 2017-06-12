using System;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Notifications.Invitations
{
    public class InvitationPartialViewModel
    {
        [NotDefault]
        public Guid InviteId { get; set; }
        public string Action { get; set; }
    }
}
