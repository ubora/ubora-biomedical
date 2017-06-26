using System;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Notifications.Invitation;
using Ubora.Domain.Notifications.Join;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Notifications.Factory
{
    public class NotificationViewModelFactory : INotificationViewModelFactory
    {
        private readonly IQueryProcessor _processor;

        public NotificationViewModelFactory(IQueryProcessor processor)
        {
            _processor = processor;
        }

        public NotificationViewModel CreateIndexViewModel(BaseNotification notification)
        {
            if (notification is InvitationToProject invitationToProject)
            {
                return CreateIndexInvitationViewModel(invitationToProject);
            }
            else if (notification is RequestToJoinProject requestToJoinProject)
            {
                return CreateIndexRequestViewModel(requestToJoinProject);
            }

            throw new ArgumentException(nameof(notification));
        }

        public NotificationViewModel CreateHistoryViewModel(BaseNotification notification)
        {
            if (notification is InvitationToProject invitationToProject)
            {
                return CreateHistoryInvitationViewModel(invitationToProject);
            }
            else if (notification is RequestToJoinProject requestToJoinProject)
            {
                return CreateHistoryRequestViewModel(requestToJoinProject);
            }

            throw new ArgumentException(nameof(notification));
        }

        private NotificationViewModel CreateHistoryRequestViewModel(RequestToJoinProject requestToJoinProject)
        {
            var project = _processor.FindById<Project>(requestToJoinProject.ProjectId);
            var fullName = _processor.FindById<UserProfile>(requestToJoinProject.AskingToJoinMemberId).FullName;

            var viewModel = new HistoryRequestViewModel
            {
                ProjectTitle = project.Title,
                ProjectId = project.Id,
                WasAccepted = requestToJoinProject.IsAccepted.Value,
                UserFullName = fullName,
                RequestToJoinProjectId = requestToJoinProject.Id,
            };

            return viewModel;
        }

        private NotificationViewModel CreateHistoryInvitationViewModel(InvitationToProject invitationToProject)
        {
            var project = _processor.FindById<Project>(invitationToProject.ProjectId);
            var fullName = _processor.FindById<UserProfile>(invitationToProject.InvitedMemberId).FullName;

            var viewModel = new HistoryInvitationViewModel
            {
                ProjectTitle = project.Title,
                ProjectId = project.Id,
                WasAccepted = invitationToProject.IsAccepted.Value,
                InviteId = invitationToProject.Id
            };

            return viewModel;
        }

        private NotificationViewModel CreateIndexRequestViewModel(RequestToJoinProject requestToJoinProject)
        {
            var project = _processor.FindById<Project>(requestToJoinProject.ProjectId);
            var fullName = _processor.FindById<UserProfile>(requestToJoinProject.AskingToJoinMemberId).FullName;

            var viewModel = new IndexRequestViewModel
            {
                ProjectTitle = project.Title,
                ProjectId = project.Id,
                IsUnread = !requestToJoinProject.HasBeenViewed,
                UserFullName = fullName,
                RequestToJoinProjectId = requestToJoinProject.Id,
            };

            return viewModel;
        }

        private NotificationViewModel CreateIndexInvitationViewModel(InvitationToProject invitationToProject)
        {
            var project = _processor.FindById<Project>(invitationToProject.ProjectId);
            var fullName = _processor.FindById<UserProfile>(invitationToProject.InvitedMemberId).FullName;

            var viewModel = new IndexInvitationViewModel
            {
                ProjectTitle = project.Title,
                InviteId = invitationToProject.Id,
                IsUnread = !invitationToProject.HasBeenViewed,
                ProjectId = project.Id,
            };

            return viewModel;
        }
    }

    public interface INotificationViewModelFactory
    {
        NotificationViewModel CreateIndexViewModel(BaseNotification notification);
        NotificationViewModel CreateHistoryViewModel(BaseNotification notification);
    }
}
