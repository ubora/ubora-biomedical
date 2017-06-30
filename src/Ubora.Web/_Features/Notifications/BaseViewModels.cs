using System;

namespace Ubora.Web._Features.Notifications
{
    public class BaseInvitationViewModel : NotificationViewModel
    {
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public Guid InviteId { get; set; }
    }

    public class BaseRequestViewModel : NotificationViewModel
    {
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public Guid RequestToJoinProjectId { get; set; }
        public string UserFullName { get; set; }
    }

    public class NotificationViewModel
    {
    }
}
