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
    public class IndexViewModel
    {
        public List<NotificationViewModel> Notifications { get; set; } = new List<NotificationViewModel>();

        public class Factory
        {
            private readonly IQueryProcessor _processor;
            public Factory(IQueryProcessor processor)
            {
                _processor = processor;
            }

            protected Factory()
            {

            }

            public virtual IndexViewModel Create(Guid userId)
            {
                var indexViewModel = new IndexViewModel();
                indexViewModel.Notifications = GetIndexInvitationViewModels(userId);

                return indexViewModel;
            }

            private List<NotificationViewModel> GetIndexInvitationViewModels(Guid userId)
            {
                var notifications = _processor.Find(new HasNoNotificationsInHistory(userId));

                var notificationViewModels = new List<NotificationViewModel>();

                foreach (var notification in notifications)
                {
                    if (notification is InvitationToProject invitationToProject)
                    {
                        var project = _processor.FindById<Project>(invitationToProject.ProjectId);
                        var fullName = _processor.FindById<UserProfile>(invitationToProject.InvitedMemberId).FullName;

                        var viewModel = new IndexInvitationViewModel
                        {
                            ProjectTitle = project.Title,
                            InviteId = notification.Id,
                            IsUnread = !notification.HasBeenViewed,
                            ProjectId = project.Id,
                        };

                        notificationViewModels.Add(viewModel);
                    }
                    else if (notification is RequestToJoinProject requestToJoinProject)
                    {
                        var project = _processor.FindById<Project>(requestToJoinProject.ProjectId);
                        var fullName = _processor.FindById<UserProfile>(requestToJoinProject.AskingToJoinMemberId).FullName;

                        var viewModel = new IndexRequestViewModel
                        {
                            ProjectTitle = project.Title,
                            ProjectId = project.Id,
                            IsUnread = !notification.HasBeenViewed,
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

    public class IndexInvitationViewModel : BaseInvitationViewModel
    {
        public bool IsUnread { get; set; }
    }

    public class IndexRequestViewModel : BaseRequestViewModel
    {
        public bool IsUnread { get; set; }
    }
}