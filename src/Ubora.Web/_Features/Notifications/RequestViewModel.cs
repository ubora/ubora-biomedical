using System;

namespace Ubora.Web._Features.Notifications
{
    public abstract class RequestViewModel : NotificationViewModel
    {
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public Guid RequestId { get; set; }
        public string UserFullName { get; set; }
    }
}