using System;

namespace Ubora.Web._Features.Notifications
{
    public class IndexInvitationViewModel
    {
        public string ProjectTitle { get; set; }
        public Guid InviteId { get; set; }
        public bool IsUnread { get; set; }
    }
}