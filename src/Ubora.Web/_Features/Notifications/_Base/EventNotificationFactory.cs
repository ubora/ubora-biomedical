using System;
using Marten;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Events;
using Ubora.Web._Features._Shared.Tokens;

namespace Ubora.Web._Features.Notifications._Base
{
    public class EventNotificationViewModel : INotificationViewModel<EventNotification>
    {
        public string ProjectTitle { get; set; }
        public Guid ProjectId { get; set; }
        public IHtmlContent Message { get; set; }
        public bool IsUnread { get; set; }
        public DateTime CreatedAt { get; set; }

        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Notifications/_EventNotificationPartial.cshtml", this);
        }

        public class Factory : INotificationViewModelFactory
        {
            private readonly IDocumentSession _session;
            private readonly TokenReplacerMediator _tokenReplacerMediator;

            public Factory(IDocumentSession session, TokenReplacerMediator tokenReplacerMediator)
            {
                _session = session;
                _tokenReplacerMediator = tokenReplacerMediator;
            }

            public bool CanCreateFor(Type type)
            {
                return typeof(EventNotification).IsAssignableFrom(type);
            }

            public EventNotificationViewModel Create(EventNotification notification)
            {
                var @event = (ProjectEvent) _session.Events.Load(notification.EventId).Data;
                var project = _session.Load<Project>(@event.ProjectId);

                return new EventNotificationViewModel
                {
                    IsUnread = !notification.HasBeenViewed,
                    Message = _tokenReplacerMediator.EncodeAndReplaceAllTokens(@event.ToString()),
                    ProjectTitle = project.Title,
                    ProjectId = project.Id,
                    CreatedAt = notification.CreatedAt
                };
            }
        }
    }
}