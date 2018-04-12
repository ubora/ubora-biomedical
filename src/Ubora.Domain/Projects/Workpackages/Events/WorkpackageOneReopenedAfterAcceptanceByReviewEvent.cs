using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Members.Specifications;
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
        
        public class Notifier : UboraEventHandler<WorkpackageOneReopenedAfterAcceptanceByReviewEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            protected override void HandleCore(WorkpackageOneReopenedAfterAcceptanceByReviewEvent @event, IEvent eventWithMetadata)
            {
                var project = _documentSession.Load<Project>(@event.ProjectId);

                var notifications =
                    project
                        .GetMembers(!new HasUserIdSpec(@event.InitiatedBy.UserId))
                        .GroupBy(member => member.UserId)
                        .Select(memberGrouping => EventNotification.Create(eventWithMetadata.Data, eventWithMetadata.Id, memberGrouping.Key));
                
                _documentSession.StoreUboraNotificationsIfAny(notifications);
                _documentSession.SaveChanges();
            }
        }
    }
}
