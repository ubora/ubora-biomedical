using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Users;
using Ubora.Web._Features.Notifications._Base;

namespace Ubora.Web._Features.Notifications.Requests
{
    public class RequestToJoinProjectNotifiationViewModel : INotificationViewModel<RequestToJoinProject>
    {
        public Guid UserId { get; set; }
        public bool WasAccepted { get; set; }
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public Guid RequestId { get; set; }
        public string UserFullName { get; set; }
        public bool IsUnread { get; set; }

        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper, bool isHistory)
        {
            if (isHistory)
            {
                return htmlHelper.Partial("~/_Features/Notifications/Requests/HistoryRequestPartial.cshtml", this);
            }
            else
            {
                return htmlHelper.Partial("~/_Features/Notifications/Requests/IndexRequestPartial.cshtml", this);
            }
        }

        public class Factory : NotificationViewModelFactory<RequestToJoinProject, RequestToJoinProjectNotifiationViewModel>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public override RequestToJoinProjectNotifiationViewModel Create(RequestToJoinProject notification)
            {
                var project = _queryProcessor.FindById<Project>(notification.ProjectId);
                var invitedMemberUserProfile = _queryProcessor.FindById<UserProfile>(notification.AskingToJoinMemberId);

                var viewModel = new RequestToJoinProjectNotifiationViewModel
                {
                    ProjectTitle = project.Title,
                    ProjectId = project.Id,
                    WasAccepted = notification.IsAccepted ?? false,
                    UserFullName = invitedMemberUserProfile.FullName,
                    UserId = invitedMemberUserProfile.UserId,
                    RequestId = notification.Id,
                    IsUnread = !notification.HasBeenViewed
                };

                return viewModel;
            }
        }
    }
}