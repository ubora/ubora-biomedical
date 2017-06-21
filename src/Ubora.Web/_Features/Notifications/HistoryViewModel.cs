using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Notifications.Invitation;
using Ubora.Domain.Notifications.Join;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Notifications
{
    public class HistoryViewModel
    {
        public List<NotificationViewModel> Notifications { get; set; } = new List<NotificationViewModel>();

        public class Factory
        {
            private readonly IQueryProcessor _processor;
            public Factory(IQueryProcessor processor)
            {
                _processor = processor;
            }

            protected Factory() { }

            public virtual HistoryViewModel Create(Guid userId)
            {
                var historyViewModel = new HistoryViewModel();
                historyViewModel.Notifications = GetHistoryInvitationViewModels(userId);

                return historyViewModel;
            }

            private List<NotificationViewModel> GetHistoryInvitationViewModels(Guid userId)
            {
                var notifications = _processor.Find(new HasNotificationsInHistory(userId));
                var notificationViewModels = new List<NotificationViewModel>();

                foreach (var notification in notifications)
                {
                    if (notification is InvitationToProject invitationToProject)
                    {
                        var project = _processor.FindById<Project>(invitationToProject.ProjectId);
                        var fullName = _processor.FindById<UserProfile>(invitationToProject.InvitedMemberId).FullName;

                        var viewModel = new HistoryInvitationViewModel
                        {
                            ProjectTitle = project.Title,
                            ProjectId = project.Id,
                            WasAccepted = invitationToProject.IsAccepted.Value,
                            InviteId = notification.Id
                        };
                        notificationViewModels.Add(viewModel);
                    }
                    else if (notification is RequestToJoinProject requestToJoinProject)
                    {
                        var project = _processor.FindById<Project>(requestToJoinProject.ProjectId);
                        var fullName = _processor.FindById<UserProfile>(requestToJoinProject.AskingToJoinMemberId).FullName;

                        var viewModel = new HistoryRequestViewModel
                        {
                            ProjectTitle = project.Title,
                            ProjectId = project.Id,
                            WasAccepted = requestToJoinProject.IsAccepted.Value,
                            UserFullName = fullName,
                            RequestToJoinProjectId = notification.Id,
                        };
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
