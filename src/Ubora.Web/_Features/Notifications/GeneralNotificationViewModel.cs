using System;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Notifications;
using Ubora.Web._Features.Notifications._Base;

namespace Ubora.Web._Features.Notifications
{
    public class GeneralNotificationViewModel : INotificationViewModel<GeneralNotification>
    {
        public bool IsUnread { get; set; }
        public string Message { get; set; }

        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper, bool isHistory)
        {
            return htmlHelper.Partial("~/_Features/Notifications/_GeneralNotificationPartial.cshtml", this);
        }

        public class Factory : INotificationViewModelFactory
        {
            public bool CanCreateFor(Type type)
            {
                 return typeof(GeneralNotification).IsAssignableFrom(type);
            }

            public GeneralNotificationViewModel Create(GeneralNotification notification)
            {
                return new GeneralNotificationViewModel
                {
                    IsUnread = !notification.HasBeenViewed,
                    Message = notification.GetDescription()
                };
            }
        }
    }
}
