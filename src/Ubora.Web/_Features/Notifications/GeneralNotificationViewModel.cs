using System;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Notifications;
using Ubora.Web._Features.Notifications._Base;
using Ubora.Web._Features._Shared.Tokens;

namespace Ubora.Web._Features.Notifications
{
    public class GeneralNotificationViewModel : INotificationViewModel<GeneralNotification>
    {
        public bool IsUnread { get; set; }
        public DateTime CreatedAt { get; set; }
        public IHtmlContent Message { get; set; }

        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Notifications/_GeneralNotificationPartial.cshtml", this);
        }

        public class Factory : INotificationViewModelFactory
        {
            private readonly TokenReplacerMediator _tokenReplacerMediator;

            public Factory(TokenReplacerMediator tokenReplacerMediator)
            {
                _tokenReplacerMediator = tokenReplacerMediator;
            }

            public bool CanCreateFor(Type type)
            {
                return typeof(GeneralNotification).IsAssignableFrom(type);
            }

            public GeneralNotificationViewModel Create(GeneralNotification notification)
            {
                return new GeneralNotificationViewModel
                {
                    IsUnread = !notification.HasBeenViewed,
                    Message = _tokenReplacerMediator.EncodeAndReplaceAllTokens(notification.GetDescription()),
                    CreatedAt = notification.CreatedAt
                };
            }
        }
    }
}
