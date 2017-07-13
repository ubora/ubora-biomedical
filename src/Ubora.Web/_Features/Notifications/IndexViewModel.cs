using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Web._Features.Notifications.Factory;

namespace Ubora.Web._Features.Notifications
{
    public class IndexViewModel
    {
        public List<NotificationViewModel> Notifications { get; set; } = new List<NotificationViewModel>();

        public class Factory
        {
            private readonly IQueryProcessor _processor;
            private readonly INotificationViewModelFactory _factory;

            public Factory(
                IQueryProcessor processor,
                INotificationViewModelFactory factory)
            {
                _processor = processor;
                _factory = factory;
            }

            protected Factory()
            {

            }

            public virtual IndexViewModel Create(Guid userId)
            {
                var indexViewModel = new IndexViewModel();
                indexViewModel.Notifications = GetIndexNotificationViewModels(userId);

                return indexViewModel;
            }

            private List<NotificationViewModel> GetIndexNotificationViewModels(Guid userId)
            {
                var notifications = _processor.Find(new HasPendingNotifications(userId)).ToList();

                var notificationViewModels = new List<NotificationViewModel>();

                foreach (var notification in notifications)
                {
                    var viewModel = _factory.CreateIndexViewModel(notification);
                    if (viewModel != null)
                    {
                        notificationViewModels.Add(viewModel);
                    }
                }

                return notificationViewModels;
            }
        }
    }

    public class IndexInvitationViewModel : BaseInvitationViewModel
    {
        public bool IsUnread { get; set; }

        public override IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Notifications/Partials/IndexInvitationPartial.cshtml", this);
        }
    }

    public class IndexRequestViewModel : BaseRequestViewModel
    {
        public Guid UserId { get; set; }
        public bool IsUnread { get; set; }

        public override IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Notifications/Partials/IndexRequestPartial.cshtml", this);
        }
    }
}