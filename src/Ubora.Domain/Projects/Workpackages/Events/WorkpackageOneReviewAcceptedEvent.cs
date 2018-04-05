using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneReviewAcceptedEvent : ProjectEvent
    {
        public WorkpackageOneReviewAcceptedEvent(UserInfo initiatedBy, Guid projectId, string concludingComment, DateTimeOffset acceptedAt) 
            : base(initiatedBy, projectId)
        {
            ConcludingComment = concludingComment;
            AcceptedAt = acceptedAt;
        }

        public string ConcludingComment { get; private set; }
        public DateTimeOffset AcceptedAt { get; private set; }

        public override string GetDescription() => $"accepted work package 1 by {StringTokens.WorkpackageOneReview()}.";

        public class Notifier : EventNotifier<WorkpackageOneReviewAcceptedEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            protected override void HandleCore(WorkpackageOneReviewAcceptedEvent @event, IEvent eventWithMetadata)
            {
                var project = _documentSession.Load<Project>(@event.ProjectId);

                var notifications =
                    project.Members
                        .Where(x => x.UserId != @event.InitiatedBy.UserId)
                        .Select(projectMember => EventNotification.Create(eventWithMetadata, projectMember.UserId));
                
                _documentSession.StoreObjects(notifications);
                _documentSession.SaveChanges();
            }
        }
    }
}