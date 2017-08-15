using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Web._Features.Notifications._Base;

namespace Ubora.Web._Features.Projects.Mentors
{
    public class ProjectMentorInvitationNotificationViewModel : INotificationViewModel<ProjectMentorInvitation>
    {
        public bool IsUnread { get; set; }
        public bool WasAccepted { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public Guid InviteId { get; set; }
        public bool IsHistory { get; set; }

        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper, bool isHistory)
        {
            return htmlHelper.Partial("~/_Features/Projects/Mentors/_MentorInvitationNotificationPartial.cshtml", this);
        }

        public class Factory : NotificationViewModelFactory<ProjectMentorInvitation, ProjectMentorInvitationNotificationViewModel>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public override ProjectMentorInvitationNotificationViewModel Create(ProjectMentorInvitation notification)
            {
                var project = _queryProcessor.FindById<Project>(notification.ProjectId);

                var viewModel = new ProjectMentorInvitationNotificationViewModel
                {
                    IsUnread = !notification.HasBeenViewed,
                    WasAccepted = notification.IsAccepted ?? false,
                    ProjectId = notification.ProjectId,
                    ProjectTitle = project.Title,
                    InviteId = notification.Id,
                    IsHistory = notification.IsArchived
                };

                return viewModel;
            }
        }
    }
}