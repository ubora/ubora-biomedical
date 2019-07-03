using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Web._Features.Notifications._Base;

namespace Ubora.Web._Features.Notifications.Invitations
{
    public class InvitationToJoinProjectNotificationViewModel : INotificationViewModel<InvitationToProject>
    {
        public bool IsUnread { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool WasAccepted { get; set; }
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public Guid InvitationId { get; set; }
        public bool IsPending { get; set; }

        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Notifications/Invitations/IndexInvitationPartial.cshtml", this);
        }

        public class Factory : NotificationViewModelFactory<InvitationToProject, InvitationToJoinProjectNotificationViewModel>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public override InvitationToJoinProjectNotificationViewModel Create(InvitationToProject notification)
            {
                var project = _queryProcessor.FindById<Project>(notification.ProjectId);

                var viewModel = new InvitationToJoinProjectNotificationViewModel
                {
                    ProjectTitle = project.Title,
                    ProjectId = project.Id,
                    WasAccepted = notification.IsAccepted ?? false,
                    InvitationId = notification.Id,
                    IsUnread = !notification.HasBeenViewed,
                    IsPending = notification.IsPending,
                    CreatedAt = notification.CreatedAt
                };

                return viewModel;
            }
        }
    }
}