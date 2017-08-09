using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web._Features.Notifications
{
    public abstract class NotificationViewModel
    {
        public abstract IHtmlContent GetPartialView(IHtmlHelper htmlHelper);
    }
}