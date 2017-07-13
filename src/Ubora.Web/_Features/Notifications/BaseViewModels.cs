using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web._Features.Notifications
{
    public abstract class BaseInvitationViewModel : NotificationViewModel
    {
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public Guid InviteId { get; set; }
    }

    public abstract class BaseRequestViewModel : NotificationViewModel
    {
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public Guid RequestId { get; set; }
        public string UserFullName { get; set; }
    }

    public abstract class NotificationViewModel
    {
        public abstract IHtmlContent GetPartialView(IHtmlHelper htmlHelper);
    }
}
