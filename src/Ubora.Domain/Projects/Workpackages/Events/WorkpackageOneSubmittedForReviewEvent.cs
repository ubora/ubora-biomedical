using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Members.Specifications;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneSubmittedForReviewEvent : ProjectEvent
    {
        public WorkpackageOneSubmittedForReviewEvent(UserInfo initiatedBy, Guid projectId, Guid reviewId, DateTimeOffset submittedAt) : base(initiatedBy, projectId)
        {
            ReviewId = reviewId;
            SubmittedAt = submittedAt;
        }

        public Guid ReviewId { get; private set; }
        public DateTimeOffset SubmittedAt { get; private set; }

        public override string GetDescription() => $"submitted workpackage 1 for {StringTokens.WorkpackageOneReview()}.";
        
        public class Notifier : EventNotifier<WorkpackageOneSubmittedForReviewEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            protected override void HandleCore(WorkpackageOneSubmittedForReviewEvent @event, IEvent eventWithMetadata)
            {
                var project = _documentSession.Load<Project>(@event.ProjectId);

                var notifications = 
                    project
                        .GetMembers(new IsMentorSpec())
                        .Select(projectMember => EventNotification.Create(eventWithMetadata, projectMember.UserId));
                
                _documentSession.StoreObjects(notifications);
                _documentSession.SaveChanges();
            }
        }
    }
}