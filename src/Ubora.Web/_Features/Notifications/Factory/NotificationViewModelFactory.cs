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
            return CreateIndexViewModelInternal((dynamic)notification);
        }

        public NotificationViewModel CreateHistoryViewModel(BaseNotification notification)
        {
            return CreateHistoryViewModelInternal((dynamic)notification);
        }

        private NotificationViewModel CreateHistoryViewModelInternal(RequestToJoinProject requestToJoinProject)
        {
            var project = _processor.FindById<Project>(requestToJoinProject.ProjectId);
            var invitedMemberUserProfile = _processor.FindById<UserProfile>(requestToJoinProject.AskingToJoinMemberId);

            var viewModel = new HistoryRequestViewModel
            {
                ProjectTitle = project.Title,
                ProjectId = project.Id,
                WasAccepted = requestToJoinProject.IsAccepted.Value,
                UserFullName = invitedMemberUserProfile.FullName,
                UserId = invitedMemberUserProfile.UserId,
                RequestId = requestToJoinProject.Id,
            };

            return viewModel;
        }

        private NotificationViewModel CreateHistoryViewModelInternal(InvitationToProject invitationToProject)
        {
            var project = _processor.FindById<Project>(invitationToProject.ProjectId);

            var viewModel = new HistoryInvitationViewModel
            {
                ProjectTitle = project.Title,
                ProjectId = project.Id,
                WasAccepted = invitationToProject.IsAccepted.Value,
                InviteId = invitationToProject.Id
            };

            return viewModel;
        }

        private NotificationViewModel CreateIndexViewModelInternal(RequestToJoinProject requestToJoinProject)
        {
            var project = _processor.FindById<Project>(requestToJoinProject.ProjectId);
            var askingToJoinMemberUserProfile = _processor.FindById<UserProfile>(requestToJoinProject.AskingToJoinMemberId);

            var viewModel = new IndexRequestViewModel
            {
                ProjectTitle = project.Title,
                ProjectId = project.Id,
                IsUnread = !requestToJoinProject.HasBeenViewed,
                UserFullName = askingToJoinMemberUserProfile.FullName,
                UserId = askingToJoinMemberUserProfile.UserId,
                RequestId = requestToJoinProject.Id,
            };

            return viewModel;
        }

        private NotificationViewModel CreateIndexViewModelInternal(InvitationToProject invitationToProject)
        {
            var project = _processor.FindById<Project>(invitationToProject.ProjectId);

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
