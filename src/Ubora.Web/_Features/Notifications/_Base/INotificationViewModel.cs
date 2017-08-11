using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Notifications;

namespace Ubora.Web._Features.Notifications._Base
{
    public interface INotificationViewModel<T> : INotificationViewModel where T : BaseNotification
    {
    }

    public interface INotificationViewModel
    {
        IHtmlContent GetPartialView(IHtmlHelper htmlHelper, bool isHistory);
    }
}