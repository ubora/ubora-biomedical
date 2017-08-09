using System;

namespace Ubora.Web._Features.Notifications
{
    public abstract class InvitationViewModel : NotificationViewModel
    {
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public Guid InviteId { get; set; }
    }
}
