using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneReopenedAfterAcceptanceByReviewEvent : ProjectEvent
    {
        public WorkpackageOneReopenedAfterAcceptanceByReviewEvent(UserInfo initiatedBy, Guid projectId, Guid acceptedReviewId) 
            : base(initiatedBy, projectId)
        {
            AcceptedReviewId = acceptedReviewId;
        }

        public Guid AcceptedReviewId { get; private set; }

        public override string GetDescription() => "reopened WP1 for edits.";
        
        public class Notifier : EventNotifier<WorkpackageOneReopenedAfterAcceptanceByReviewEvent>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IQueryProcessor _queryProcessor;

            public Notifier(IDocumentSession documentSession, IQueryProcessor queryProcessor)
            {
                _documentSession = documentSession;
                _queryProcessor = queryProcessor;
            }
            
            protected override void HandleCore(WorkpackageOneReopenedAfterAcceptanceByReviewEvent @event, IEvent eventWithMetadata)
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
