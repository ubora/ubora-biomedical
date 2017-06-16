using System;

namespace Ubora.Web._Features.Notifications
{
    public class BaseInvitationViewModel
    {
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public Guid InviteId { get; set; }
        public string UserFullName { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}
