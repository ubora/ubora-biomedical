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
        
        public class Notifier : UboraEventHandler<WorkpackageOneReviewRejectedEvent>
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