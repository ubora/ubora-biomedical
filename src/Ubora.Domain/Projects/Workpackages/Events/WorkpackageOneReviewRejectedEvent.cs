using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneReviewRejectedEvent : ProjectEvent
    {
        public WorkpackageOneReviewRejectedEvent(UserInfo initiatedBy, Guid projectId, string concludingComment, DateTimeOffset rejectedAt) : base(initiatedBy, projectId)
        {
            ConcludingComment = concludingComment;
            RejectedAt = rejectedAt;
        }

        public string ConcludingComment { get; private set; }
        public DateTimeOffset RejectedAt { get; private set; }

        public override string GetDescription() => $"rejected workpackage 1 by {StringTokens.WorkpackageOneReview()}.";
        
        public class Notifier : EventNotifier<WorkpackageOneReviewRejectedEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            protected override void HandleCore(WorkpackageOneReviewRejectedEvent @event, IEvent eventWithMetadata)
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