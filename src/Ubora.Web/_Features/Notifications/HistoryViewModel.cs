using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Web._Features.Notifications.Factory;

namespace Ubora.Web._Features.Notifications
{
    public class HistoryViewModel
    {
        public List<NotificationViewModel> Notifications { get; set; } = new List<NotificationViewModel>();

        public class Factory
        {
            private readonly IQueryProcessor _processor;
            private readonly INotificationViewModelFactory _notificationViewModelFactory;

            public Factory(IQueryProcessor processor, INotificationViewModelFactory notificationViewModelFactory)
            {
                _processor = processor;
                _notificationViewModelFactory = notificationViewModelFactory;
            }

            protected Factory() { }

            public virtual HistoryViewModel Create(Guid userId)
            {
                var historyViewModel = new HistoryViewModel();
                historyViewModel.Notifications = GetHistoryNotificationViewModels(userId);

                return historyViewModel;
            }

            private List<NotificationViewModel> GetHistoryNotificationViewModels(Guid userId)
            {
                var notifications = _processor.Find(new HasArchivedNotifications(userId));
                var notificationViewModels = new List<NotificationViewModel>();

                foreach (var notification in notifications)
                {
                    var viewModel = _notificationViewModelFactory.CreateHistoryViewModel(notification);
                    if (viewModel != null)
                    {
                        notificationViewModels.Add(viewModel);
                    }
                }

                return notificationViewModels;
            }
        }
    }

    public class HistoryInvitationViewModel : BaseInvitationViewModel
    {
        public bool WasAccepted { get; set; }
    }

    public class HistoryRequestViewModel : BaseRequestViewModel
    {
        public bool WasAccepted { get; set; }
    }
}
